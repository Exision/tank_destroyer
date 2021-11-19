using Actors;
using Hero;
using Infrastructure.StateMachine.Game;
using Infrastructure.StateMachine.Spawner;
using Logic;
using Services;
using StaticData;
using UnityEngine;
using Weapons;
using Zenject;

namespace Infrastructure
{
    public class LevelInstaller: MonoInstaller<LevelInstaller>, IInitializable
    {
        [SerializeField] private LevelStaticData _levelData;
        [SerializeField] private PlayerStaticData _playerData;
        [SerializeField] private Spawner _enemySpawner;
        
        public override void InstallBindings()
        {
            BindInterfaces();

            BindLevelData();
            BindPlayerData();
            BindSpawner();
        }

        public void Initialize()
        {
            BindPlayer();
            BindEnemyFactory();
            BindGameStateMachineLevelStates();
            BindSpawnerStateMachineStates();
        }

        private void BindInterfaces() => 
            Container.BindInterfacesTo<LevelInstaller>().FromInstance(this).AsSingle();

        private void BindLevelData() => 
            Container.Bind<LevelStaticData>()
                .FromInstance(_levelData)
                .AsSingle();

        private void BindPlayerData() => 
            Container.Bind<PlayerStaticData>()
                .FromInstance(_playerData)
                .AsSingle();

        private void BindSpawner()
        {
            Container.Bind<Spawner>()
                .FromInstance(_enemySpawner)
                .AsSingle();
            
            Container.Bind<SpawnerStateMachine>()
                .FromInstance(new SpawnerStateMachine(new SpawnerStateFactory(Container)))
                .AsSingle();
        }

        private void BindPlayer()
        {
            PlayerHealth player = Container.
                InstantiatePrefabForComponent<PlayerHealth>(
                    _playerData.Prefab,
                    _levelData.PlayerStartPoint,
                    Quaternion.identity, 
                    null);
            
            player.Initialize(_playerData.Health, _playerData.Armor);
            
            player.GetComponent<PlayerAttack>()
                .Initialize(CreateWeapons(_playerData.AvailableWeapons, player.transform));
            
            player.GetComponent<PlayerMove>()
                .Initialize(_playerData.MoveSpeed, _playerData.RotationSpeed);
            
            player.GetComponent<ActorUi>()
                .Initialize(player);

            player.gameObject.SetActive(false);
            
            Container.Bind<PlayerHealth>()
                .FromInstance(player)
                .AsSingle();
        }

        private Weapon[] CreateWeapons(WeaponStaticData[] weaponsData, Transform player)
        {
            Weapon[] weapons = new Weapon[weaponsData.Length];
            
            for (var index = 0; index < weapons.Length; index++)
            {
                WeaponStaticData weaponData = weaponsData[index];
                
                Weapon weapon = Container.InstantiatePrefabForComponent<Weapon>(
                    weaponData.WeaponPrefab,
                    Vector3.zero,
                    Quaternion.identity, 
                    player);
                
                weapon.Initialize(
                    weaponData.Damage,
                    weaponData.RateOfFire, 
                    weaponData.BulletSpeed, 
                    weaponData.BulletPrefab);
                
                weapon.gameObject.SetActive(false);
                
                weapons[index] = weapon;
            }

            return weapons;
        }

        private void BindEnemyFactory()
        {
            var enemyFactory = new EnemyFactory(Container.Resolve<PlayerHealth>());
            Container.Bind<IEnemyFactory>()
                .FromInstance(enemyFactory)
                .AsSingle();
        }

        private void BindGameStateMachineLevelStates()
        {
            var startGameState = new StartGameState(
                Container.Resolve<GameStateMachine>(),
                Container.Resolve<SpawnerStateMachine>(),
                Container.Resolve<PlayerHealth>());
            
            Container.Bind<StartGameState>()
                .FromInstance(startGameState)
                .AsTransient();

            var waitToGameEndState = new WaitToGameEndState(Container.Resolve<GameStateMachine>(),
                Container.Resolve<SpawnerStateMachine>(),
                Container.Resolve<PlayerHealth>(),
                Container.Resolve<LevelStaticData>());
            
            Container.Bind<WaitToGameEndState>()
                .FromInstance(waitToGameEndState)
                .AsTransient();
            
            GameStateFactory gameStateFactory = Container.Resolve<GameStateFactory>();
            gameStateFactory.AddAdditionalStates(Container);
        }

        private void BindSpawnerStateMachineStates()
        {
            var spawnerInitializeState = new SpawnerInitializeState(
                Container.Resolve<SpawnerStateMachine>(), 
                Container.Resolve<LevelStaticData>(), 
                Container.Resolve<IEnemyFactory>());
            
            Container.Bind<SpawnerInitializeState>()
                .FromInstance(spawnerInitializeState)
                .AsSingle();

            var spawnEnemyState = new SpawnEnemyState(
                Container.Resolve<SpawnerStateMachine>(), 
                Container.Resolve<Spawner>(), 
                Container.Resolve<IEnemyFactory>(), 
                Container.Resolve<LevelStaticData>(), 
                Container.Resolve<IRandomService>());
            
            Container.Bind<SpawnEnemyState>()
                .FromInstance(spawnEnemyState)
                .AsSingle();

            var spawnDelayState = new SpawnDelayState(
                Container.Resolve<SpawnerStateMachine>(), 
                Container.Resolve<LevelStaticData>(), 
                Container.Resolve<ICoroutineRunner>());
            
            Container.Bind<SpawnDelayState>()
                .FromInstance(spawnDelayState)
                .AsSingle();

            var waitToSpawnState = new WaitToSpawnState(
                Container.Resolve<SpawnerStateMachine>(), 
                Container.Resolve<IEnemyFactory>());

            Container.Bind<WaitToSpawnState>()
                .FromInstance(waitToSpawnState)
                .AsSingle();

            var stopSpawnState = new StopSpawnState(Container.Resolve<IEnemyFactory>());
            Container.Bind<StopSpawnState>()
                .FromInstance(stopSpawnState)
                .AsSingle();
        }
    }
}