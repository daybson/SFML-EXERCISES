using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLFramework.src.Helper
{
    public static class GameObjectCreator
    {
        #region Fields

        private static readonly Material inelasticMaterial = new Material("Inelastic", 8, 1, 1, ECollisionType.Inelastic);
        private static readonly Material elasticMaterial = new Material("Elastic", 8, 0, 1, ECollisionType.Elastic);
        private static readonly Material partialInelasticMaterial = new Material("PartialInelastic", 8, 0.5f, 1, ECollisionType.PartialInelastic);

        #endregion


        #region Public

        public static GameObject CreateInelasticBrick(Vector2f position)
        {
            var inelasticBrick = new GameObject("InelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("inelasticBrick.png"), inelasticBrick);
            inelasticBrick.Components.Add(renderer);
            inelasticBrick.Components.Add(new Rigidbody(1, renderer.SpriteSheet.Size, inelasticMaterial, false, inelasticBrick, V2.One * 50));
            inelasticBrick.Position = position;
            return inelasticBrick;
        }

        public static GameObject CreateElasticBrick(Vector2f position)
        {
            var elasticBrick = new GameObject("ElasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("elasticBrick.png"), elasticBrick);
            //TODO: passar Add na lista de componentes para construtor do componente -> evita chamada externa?
            elasticBrick.Components.Add(renderer);

            var rigidbody = new Rigidbody(25, renderer.SpriteSheet.Size, elasticMaterial, false, elasticBrick, V2.One * 250);
            elasticBrick.Components.Add(rigidbody);

            var label = new UIText(elasticBrick, new Vector2i(0, -28));
            elasticBrick.Components.Add(label);
            label.Display = (v) => label.SetMessage(string.Format("Vx: {0}\nVy: {1}", rigidbody.Velocity.X.ToString("0.0"), rigidbody.Velocity.Y.ToString("0.0")));

            elasticBrick.Position = position;
            return elasticBrick;
        }

        public static GameObject CreatePartialInelasticBrick(Vector2f position)
        {
            var partialInelasticBrick = new GameObject("PartialInelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("partialInelasticBrick.png"), partialInelasticBrick);
            partialInelasticBrick.Components.Add(renderer);
            partialInelasticBrick.Components.Add(new Rigidbody(2, renderer.SpriteSheet.Size, partialInelasticMaterial, false, partialInelasticBrick, new Vector2f(50, 190)));
            partialInelasticBrick.Position = position;
            return partialInelasticBrick;
        }

        public static GameObject CreatePlatform(EDirection direction, Vector2f position)
        {
            var platform = new GameObject("Platform" + direction.ToString());
            string sprite = (direction == EDirection.Left || direction == EDirection.Right) ? "platformSide.png" : "platform.png";
            var platformRenderer = new Renderer(Resources.LoadSpriteSheet(sprite), platform);
            platform.Components.Add(platformRenderer);
            platform.Components.Add(new Rigidbody(9, platformRenderer.SpriteSheet.Size, inelasticMaterial, true, platform, V2.One * 50));
            platform.Position = position;
            return platform;
        }

        public static Player CreatePlayer(ref KeyboardInput keyboard)
        {
            var player = new Player();
            player.SetKeyboardInput(ref keyboard);
            player.PlatformPlayerController.PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.A, () => player.PlatformPlayerController.Walk(EDirection.Left, true));
            player.PlatformPlayerController.PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.D, () => player.PlatformPlayerController.Walk(EDirection.Right, true));
            player.PlatformPlayerController.PlayerKeyboardController.keyReleasedActions.Add(Keyboard.Key.A, () => player.PlatformPlayerController.Walk(EDirection.Left, false));
            player.PlatformPlayerController.PlayerKeyboardController.keyReleasedActions.Add(Keyboard.Key.D, () => player.PlatformPlayerController.Walk(EDirection.Right, false));
            player.PlatformPlayerController.PlayerKeyboardController.keyPressedActions.Add(Keyboard.Key.Space, () => player.PlatformPlayerController.Jump());
            player.Rigidbody.OnCollisionResponse += player.PlatformPlayerController.OnCollisionResponse;
            return player;
        }

        #endregion
    }
}