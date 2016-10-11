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
                    elasticBrick,
                    new Vector2f(50, 50)));
            elasticBrick.Position = new Vector2f(120, 185);
            return elasticBrick;
        }

        public static GameObject CreateInelasticBrick(Vector2f position)
        {
            var inelasticBrick = new GameObject("InelasticBrick");
            inelasticBrick.Position = position;

            var renderer = new Renderer(Resources.LoadSpriteSheet("inelasticBrick.png"), inelasticBrick);
            inelasticBrick.Components.Add(renderer);
            inelasticBrick.Subscribe(renderer);
            inelasticBrick.Components.Add(
                new Rigidbody(
                    9,
                    0,
                    new Vector2f(renderer.SpriteSheet.TileWidth, renderer.SpriteSheet.TileHeight),
                    new Material("InelasticBrickRigidBody", 8, 1, 1, ECollisionType.Inelastic),
                    false,
                    inelasticBrick,
                    new Vector2f(50, 50)));
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
                    false,
                    partialInelasticBrick,
                    new Vector2f(50, 100)));
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
            return player;
        }

        public static GameObject CreatePlatform(EDirection direction, Vector2f position)
        {
            var platform = new GameObject("Platform" + direction.ToString());

            string sprite = "";
            if (direction == EDirection.Left || direction == EDirection.Right)
                sprite = "platformSide.png";
            else
                sprite = "platform.png";
            platform.Position = position;

            var platformRenderer = new Renderer(Resources.LoadSpriteSheet(sprite), platform);
            platform.Components.Add(platformRenderer);
            platform.Subscribe(platformRenderer);
            platform.Components.Add(
                new Rigidbody(
                    9,
                    0,
                    new Vector2f(platformRenderer.SpriteSheet.TileWidth, platformRenderer.SpriteSheet.TileHeight),
                    new Material("RPlatform" + direction.ToString(), 1, 1, 1, ECollisionType.Inelastic),
                    true,
                    platform,
                    new Vector2f(50, 50)));

            return platform;
        }


    }
}