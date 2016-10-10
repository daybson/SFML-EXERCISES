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
        public static GameObject CreateElasticBrick()
        {
            var elasticBrick = new GameObject("ElasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("elasticBrick.png"), elasticBrick);
            elasticBrick.Components.Add(renderer);
            elasticBrick.Subscribe(renderer);
            elasticBrick.Components.Add(
                new Rigidbody(
                    5,
                    0.5f,
                    new Vector2f(renderer.SpriteSheet.TileWidth, renderer.SpriteSheet.TileHeight),
                    new Material("ElasticBrick", 8, 1, 1, ECollisionType.Elastic),
                    false,
                    elasticBrick));
            elasticBrick.Position = new Vector2f(120, 185);
            return elasticBrick;
        }

        public static GameObject CreateInelasticBrick()
        {
            var inelasticBrick = new GameObject("InelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("inelasticBrick.png"), inelasticBrick);
            inelasticBrick.Components.Add(renderer);
            inelasticBrick.Subscribe(renderer);
            inelasticBrick.Components.Add(
                new Rigidbody(
                    9,
                    0,
                    new Vector2f(renderer.SpriteSheet.TileWidth, renderer.SpriteSheet.TileHeight),
                    new Material("InelasticBrick", 8, 1, 1, ECollisionType.Inelastic),
                    false,
                    inelasticBrick));
            inelasticBrick.Position = new Vector2f(120, 185);
            return inelasticBrick;
        }

        public static GameObject CreatePartialInelasticBrick()
        {
            var partialInelasticBrick = new GameObject("PartialInelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("partialInelasticBrick.png"), partialInelasticBrick);
            partialInelasticBrick.Components.Add(renderer);
            partialInelasticBrick.Subscribe(renderer);
            partialInelasticBrick.Components.Add(
                new Rigidbody(
                    9,
                    0,
                    new Vector2f(renderer.SpriteSheet.TileWidth, renderer.SpriteSheet.TileHeight),
                    new Material("PartialInelasticBrick", 8, 1, 1, ECollisionType.PartialInelastic),
                    true,
                    partialInelasticBrick));
            partialInelasticBrick.Position = new Vector2f(120, 185);
            return partialInelasticBrick;
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
            player.Position = new Vector2f(340, 100);
            return player;
        }

        public static GameObject CreatePlatform()
        {
            var platform = new GameObject("Platform");

            var platformRenderer = new Renderer(Resources.LoadSpriteSheet("platform.png"), platform);
            platform.Components.Add(platformRenderer);
            platform.Subscribe(platformRenderer);
            platform.Components.Add(
                new Rigidbody(
                    0,
                    0,
                    new Vector2f(platformRenderer.SpriteSheet.TileWidth, platformRenderer.SpriteSheet.TileHeight),
                    new Material("Platform", 1, 1, 1, ECollisionType.Inelastic),
                    true,
                    platform));
            platform.Position = new Vector2f(0, 230);

            return platform;
        }
    }
}