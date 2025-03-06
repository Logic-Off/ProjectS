using System.Xml;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectDawn.Navigation.Hybrid
{
    public interface INavMeshWallProvider { }

    /// <summary>
    /// Agent uses NavMesh for pathfinding.
    /// </summary>
    [RequireComponent(typeof(AgentAuthoring))]
    [AddComponentMenu("Agents Navigation/Agent NavMesh Pathing")]
    [DisallowMultipleComponent]
    [HelpURL("https://lukaschod.github.io/agents-navigation-docs/manual/game-objects/pathing/nav-mesh.html")]
    public class AgentNavMeshAuthoring : MonoBehaviour, INavMeshWallProvider
    {
        [SerializeField]
        protected int AgentTypeId = 0;

        [SerializeField]
        protected int AreaMask = -1;

        [SerializeField]
        protected bool AutoRepath = true;

        [FormerlySerializedAs("m_Constrained")]
        [SerializeField]
        protected bool m_Grounded = true;

        [SerializeField]
        internal NavMeshLinkTraversalMode m_LinkTraversalMode = NavMeshLinkTraversalMode.None;

        [SerializeField]
        protected float3 MappingExtent = 10;

        Entity m_Entity;

        /// <summary>
        /// Returns default component of <see cref="NavMeshPath"/>.
        /// </summary>
        [System.Obsolete("This property has been renamed. Please use DefaultPath!")]
        public NavMeshPath DefaulPath => DefaultPath;

        /// <summary>
        /// Returns default component of <see cref="NavMeshPath"/>.
        /// </summary>
        public NavMeshPath DefaultPath => new()
        {
            State = NavMeshPathState.Finished,
            AgentTypeId = AgentTypeId,
            AreaMask = AreaMask,
            AutoRepath = AutoRepath,
            Grounded = m_Grounded,
            MappingExtent = MappingExtent,
        };

        /// <summary>
        /// <see cref="NavMeshPath"/> component of this <see cref="AgentAuthoring"/> Entity.
        /// Accessing this property is potentially heavy operation as it will require wait for agent jobs to finish.
        /// </summary>
        public NavMeshPath EntityPath
        {
            get => World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<NavMeshPath>(m_Entity);
            set => World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(m_Entity, value);
        }

        /// <summary>
        /// <see cref="NavMeshLinkTraversal"/> component of this <see cref="AgentAuthoring"/> Entity.
        /// Accessing this property is potentially heavy operation as it will require wait for agent jobs to finish.
        /// </summary>
        public bool OnLinkTraversal
        {
            get => World.DefaultGameObjectInjectionWorld.EntityManager.IsComponentEnabled<LinkTraversal>(m_Entity);
            set => World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentEnabled<LinkTraversal>(m_Entity, value);
        }

        public ref LinkTraversalSeek SeekLinkTraversal => ref World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentDataRW<LinkTraversalSeek>(m_Entity).ValueRW;

        public ref NavMeshLinkTraversal NavMeshLinkTraversal => ref World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentDataRW<NavMeshLinkTraversal>(m_Entity).ValueRW;

        /// <summary>
        /// <see cref="NavMeshNode"/> component of this <see cref="AgentAuthoring"/> Entity.
        /// Accessing this property is potentially heavy operation as it will require wait for agent jobs to finish.
        /// </summary>
        public DynamicBuffer<NavMeshNode> EntityNodes => World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<NavMeshNode>(m_Entity);

        /// <summary>
        /// Returns true if <see cref="AgentAuthoring"/> entity has <see cref="NavMeshPath"/>.
        /// </summary>
        public bool HasEntityPath => World.DefaultGameObjectInjectionWorld != null && World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<NavMeshPath>(m_Entity);

        void Awake()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            m_Entity = GetComponent<AgentAuthoring>().GetOrCreateEntity();
            world.EntityManager.AddComponentData(m_Entity, DefaultPath);
            world.EntityManager.AddBuffer<NavMeshNode>(m_Entity);

            // Sync in case it was created as disabled
            if (!enabled)
                world.EntityManager.SetComponentEnabled<NavMeshPath>(m_Entity, false);

            if (m_LinkTraversalMode != NavMeshLinkTraversalMode.None)
            {
                world.EntityManager.AddComponent<LinkTraversal>(m_Entity);
                world.EntityManager.SetComponentEnabled<LinkTraversal>(m_Entity, false);
            }
            if (m_LinkTraversalMode == NavMeshLinkTraversalMode.Seeking)
                world.EntityManager.AddComponent<LinkTraversalSeek>(m_Entity);
            if (m_LinkTraversalMode == NavMeshLinkTraversalMode.Custom)
                world.EntityManager.AddComponent<NavMeshLinkTraversal>(m_Entity);
        }

        void OnDestroy()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world != null)
            {
                world.EntityManager.RemoveComponent<NavMeshPath>(m_Entity);
                world.EntityManager.RemoveComponent<NavMeshNode>(m_Entity);
                if (m_LinkTraversalMode != NavMeshLinkTraversalMode.None)
                    world.EntityManager.RemoveComponent<LinkTraversal>(m_Entity);
                if (m_LinkTraversalMode == NavMeshLinkTraversalMode.Seeking)
                    world.EntityManager.RemoveComponent<LinkTraversalSeek>(m_Entity);
                if (m_LinkTraversalMode == NavMeshLinkTraversalMode.Custom)
                    world.EntityManager.RemoveComponent<NavMeshLinkTraversal>(m_Entity);
            }
        }

        void OnEnable()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
                return;
            world.EntityManager.SetComponentEnabled<NavMeshPath>(m_Entity, true);
        }

        void OnDisable()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
                return;
            world.EntityManager.SetComponentEnabled<NavMeshPath>(m_Entity, false);
        }
    }

    internal class AgentNavMeshBaker : Baker<AgentNavMeshAuthoring>
    {
        public override void Bake(AgentNavMeshAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.DefaultPath);
            AddBuffer<NavMeshNode>(entity);
            if (authoring.m_LinkTraversalMode != NavMeshLinkTraversalMode.None)
            {
                AddComponent<LinkTraversal>(entity);
                SetComponentEnabled<LinkTraversal>(entity, false);
            }
            if (authoring.m_LinkTraversalMode == NavMeshLinkTraversalMode.Seeking)
                AddComponent<LinkTraversalSeek>(entity);
            if (authoring.m_LinkTraversalMode == NavMeshLinkTraversalMode.Custom)
                AddComponent<NavMeshLinkTraversal>(entity);
        }
    }
}
