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
        private static readonly Material inelasticMaterial = new Material("Inelastic", 8, 1, 1, ECollisionType.Inelastic);
        private static readonly Material elasticMaterial = new Material("Elastic", 8, 1, 1, ECollisionType.Elastic);
        private static readonly Material partialInelasticMaterial = new Material("PartialInelastic", 8, 1, 1, ECollisionType.PartialInelastic);


        public static GameObject CreateInelasticBrick(Vector2f position)
        {
            var inelasticBrick = new GameObject("InelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("inelasticBrick.png"), inelasticBrick);
            inelasticBrick.Components.Add(renderer);
            inelasticBrick.Components.Add(new Rigidbody(1, 0, renderer.SpriteSheet.Size, inelasticMaterial, false, inelasticBrick, V2.One * 50));
            inelasticBrick.Position = position;
            return inelasticBrick;
        }
        /*
        public static GameObject CreateElasticBrick()
        {
            var elasticBrick = new GameObject("ElasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("elasticBrick.png"), elasticBrick);
            elasticBrick.Components.Add(renderer);
            elasticBrick.Components.Add(new Rigidbody(5, 0.5f, renderer.SpriteSheet.Size, elasticMaterial, false, elasticBrick, V2.One * 50));
            elasticBrick.Position = new Vector2f(120, 185);
            return elasticBrick;
        }

        public static GameObject CreatePartialInelasticBrick()
        {
            var partialInelasticBrick = new GameObject("PartialInelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("partialInelasticBrick.png"), partialInelasticBrick);
            partialInelasticBrick.Components.Add(renderer);
            partialInelasticBrick.Components.Add(new Rigidbody(9, 0, renderer.SpriteSheet.Size, partialInelasticMaterial, false, partialInelasticBrick, new Vector2f(50, 100)));
            partialInelasticBrick.Position = new Vector2f(120, 185);
            return partialInelasticBrick;
        }
        */
        public static GameObject CreatePlatform(EDirection direction, Vector2f position)
        {
            var platform = new GameObject("Platform" + direction.ToString());
            string sprite = (direction == EDirection.Left || direction == EDirection.Right) ? "platformSide.png" : "platform.png";
            var platformRenderer = new Renderer(Resources.LoadSpriteSheet(sprite), platform);
            platform.Components.Add(platformRenderer);
            platform.Components.Add(new Rigidbody(9, 0, platformRenderer.SpriteSheet.Size, inelasticMaterial, true, platform, V2.One * 50));
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
    }
}