using SFML.System;
using SFML.Window;

namespace SFMLFramework.src.Helper
{
    /// <summary>
    /// Classe auxiliar para criação rápida de objetos no jogo
    /// </summary>
    public static class GameObjectCreator
    {
        #region Fields

        public static readonly Material InelasticMaterial = new Material("Inelastic", 0.9f, ECollisionType.Inelastic);
        public static readonly Material ElasticMaterial = new Material("Elastic", 1, ECollisionType.Elastic);
        public static readonly Material PartialInelasticMaterial = new Material("PartialInelastic", 0.4f, ECollisionType.PartialInelastic);

        #endregion


        #region Public

        public static GameObject CreateInelasticBrick(Vector2f position)
        {
            var inelasticBrick = new GameObject("InelasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("inelasticBrick.png"), inelasticBrick);
            inelasticBrick.Components.Add(renderer);

            var rigidbody = new Rigidbody(3f, renderer.SpriteSheet.Size, InelasticMaterial, false, inelasticBrick, V2.One * 50);
            inelasticBrick.Components.Add(rigidbody);

            var label = new UIText(inelasticBrick, new Vector2i(0, -28));
            inelasticBrick.Components.Add(label);
            label.Display = (v) => label.SetMessage(string.Format("Vx: {0}\nVy: {1}", rigidbody.Velocity.X.ToString("0.0"), rigidbody.Velocity.Y.ToString("0.0")));

            inelasticBrick.Position = position;

            return inelasticBrick;
        }

        public static GameObject CreateElasticBrick(Vector2f position)
        {
            var elasticBrick = new GameObject("ElasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("elasticBrick.png"), elasticBrick);
            elasticBrick.Components.Add(renderer);

            var rigidbody = new Rigidbody(1, renderer.SpriteSheet.Size, ElasticMaterial, false, elasticBrick, V2.One * 250);
            elasticBrick.Components.Add(rigidbody);

            var label = new UIText(elasticBrick, new Vector2i(0, -28));
            elasticBrick.Components.Add(label);
            label.Display = (v) => label.SetMessage(string.Format("Vx: {0}\nVy: {1}", rigidbody.Velocity.X.ToString("0.0"), rigidbody.Velocity.Y.ToString("0.0")));

            elasticBrick.Position = position;

            return elasticBrick;
        }

        public static GameObject CreateElasticBrick2(Vector2f position)
        {
            var elasticBrick = new GameObject("ElasticBrick");
            var renderer = new Renderer(Resources.LoadSpriteSheet("elasticBrick.png"), elasticBrick);
            elasticBrick.Components.Add(renderer);

            var rigidbody = new Rigidbody(8, renderer.SpriteSheet.Size, ElasticMaterial, false, elasticBrick, V2.One * 250);
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
            var rigidbody = new Rigidbody(2, renderer.SpriteSheet.Size, PartialInelasticMaterial, false, partialInelasticBrick, new Vector2f(250, 190));
            partialInelasticBrick.Components.Add(rigidbody);

            var label = new UIText(partialInelasticBrick, new Vector2i(0, -28));
            partialInelasticBrick.Components.Add(label);
            label.Display = (v) => label.SetMessage(string.Format("Vx: {0}\nVy: {1}", rigidbody.Velocity.X.ToString("0.0"), rigidbody.Velocity.Y.ToString("0.0")));

            partialInelasticBrick.Position = position;
            return partialInelasticBrick;
        }

        public static GameObject CreatePlatform(EDirection direction, Vector2f position)
        {
            var platform = new GameObject("Platform" + direction.ToString());
            string sprite = (direction == EDirection.Left || direction == EDirection.Right) ? "platformSide.png" : "platform.png";
            var platformRenderer = new Renderer(Resources.LoadSpriteSheet(sprite), platform);
            platform.Components.Add(platformRenderer);
            platform.Components.Add(new Rigidbody(9, platformRenderer.SpriteSheet.Size, InelasticMaterial, true, platform, V2.One * 50));
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