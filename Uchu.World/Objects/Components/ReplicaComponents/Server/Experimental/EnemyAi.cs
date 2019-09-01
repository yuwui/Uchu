using System.Numerics;

namespace Uchu.World.Experimental
{
    public class EnemyAi : Component
    {
        private ControllablePhysicsComponent _controllablePhysics;

        private BaseCombatAiComponent _baseCombatAi;

        private int _ticks;

        public bool FallowPlayer { get; set; }
        
        public Vector3 TargetLocation { get; set; }
        
        public float Speed { get; set; }
        
        public Vector3 FallowLocation { get; private set; }

        public override void Instantiated()
        {
            _baseCombatAi = GameObject.GetComponent<BaseCombatAiComponent>();
            _controllablePhysics = GameObject.GetComponent<ControllablePhysicsComponent>();
        }

        public override void Update()
        {
            var targetLocation = TargetLocation;

            if (FallowPlayer)
            {
                Player target = default;

                foreach (var player in Zone.Players)
                {
                    if (ReferenceEquals(target, default))
                    {
                        target = player;
                        continue;
                    }

                    if (Vector3.Distance(target.Transform.Position, Transform.Position) >
                        Vector3.Distance(player.Transform.Position, Transform.Position)) target = player;
                }

                _baseCombatAi.Target = target;
                
                if (ReferenceEquals(target, default))
                {
                    _baseCombatAi.PerformingAction = false;
                    _baseCombatAi.Action = CombatAiAction.None;
                    return;
                }
                
                targetLocation = target.Transform.Position;
                
                Transform.Rotation = target.Transform.Rotation;
            }
            
            _baseCombatAi.PerformingAction = true;
            
            _baseCombatAi.Action = CombatAiAction.Attacking;
            
            _ticks++;
            
            Transform.Position = Transform.Position.MoveTowards(targetLocation, Speed * Zone.TimeDelta);
            
            _controllablePhysics.HasPosition = true;
            
            _controllablePhysics.Velocity = Vector3.Normalize(Transform.Position - targetLocation);

            FallowLocation = targetLocation;
            
            if (_ticks == 5)
            {
                GameObject.Serialize();
                _ticks = default;
            }
        }
    }
}