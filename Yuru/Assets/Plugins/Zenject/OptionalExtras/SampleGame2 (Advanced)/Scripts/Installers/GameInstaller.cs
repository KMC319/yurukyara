﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree.Util;
using UnityEngine;
using Zenject;

namespace Zenject.SpaceFighter
{
    // Main installer for our game
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        Settings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemySpawner>().AsSingle();

            Container.BindFactory<float, float, EnemyFacade, EnemyFacade.Factory>()
                .FromPoolableMemoryPool<EnemyFacade, EnemyFacadePool>(poolBinder => poolBinder
                    // Spawn 5 enemies right off the bat so that we don't incur spikes at runtime
                    .WithInitialSize(5)
                    .FromSubContainerResolve()
                    .ByNewPrefabInstaller<EnemyInstaller>(_settings.EnemyFacadePrefab)
                    // Place each enemy under an Enemies game object at the root of scene hierarchy
                    .UnderTransformGroup("Enemies"));

            Container.BindFactory<float, float, BulletTypes, Bullet, Bullet.Factory>()
                .FromPoolableMemoryPool<Bullet, BulletPool>(poolBinder => poolBinder
                    // Spawn 20 right off the bat so that we don't incur spikes at runtime
                    .WithInitialSize(20)
                    // Bullets are simple enough that we don't need to make a subcontainer for them
                    // The logic can all just be in one class
                    .FromComponentInNewPrefab(_settings.BulletPrefab)
                    .UnderTransformGroup("Bullets"));

            Container.Bind<LevelBoundary>().AsSingle();

            Container.BindFactory<Explosion, Explosion.Factory>()
                .FromPoolableMemoryPool<Explosion, ExplosionPool>(poolBinder => poolBinder
                    // Spawn 4 right off the bat so that we don't incur spikes at runtime
                    .WithInitialSize(4)
                    .FromComponentInNewPrefab(_settings.ExplosionPrefab)
                    .UnderTransformGroup("Explosions"));

            Container.Bind<AudioPlayer>().AsSingle();

            Container.BindInterfacesTo<GameRestartHandler>().AsSingle();

            Container.Bind<EnemyRegistry>().AsSingle();

            GameSignalsInstaller.Install(Container);
        }

        [Serializable]
        public class Settings
        {
            public GameObject EnemyFacadePrefab;
            public GameObject BulletPrefab;
            public GameObject ExplosionPrefab;
        }

        class EnemyFacadePool : MonoPoolableMemoryPool<float, float, IMemoryPool, EnemyFacade>
        {
        }

        class BulletPool : MonoPoolableMemoryPool<float, float, BulletTypes, IMemoryPool, Bullet>
        {
        }

        class ExplosionPool : MonoPoolableMemoryPool<IMemoryPool, Explosion>
        {
        }
    }
}
