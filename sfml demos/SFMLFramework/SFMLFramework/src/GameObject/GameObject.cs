using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFMLFramework.src;
using SFML.System;

namespace SFMLFramework
{
    /// <summary>
    /// Define um objeto do game, com eventos de interação com o gameloop e propriedades de controle universais
    /// </summary>
    public class GameObject : Observable<GameObject>
    {
        /// <summary>
        /// GameObject está habilitado? (somente executa Update caso esteja)
        /// </summary>
        protected bool isEnabled;

        /// <summary>
        /// Nome do objeto
        /// </summary>
        protected string name;

        /// <summary>
        /// Id único do objeto
        /// </summary>
        protected int id;

        protected Vector2f position;

        public Vector2f Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                Notify(this);
            }
        }

        /// <summary>
        /// Cosntrutor padrão
        /// </summary>
        public GameObject()
        {
        }

        /// <summary>
        /// Lista de componentes adicionados ao gameobject
        /// </summary>
        public List<IComponent> Components = new List<IComponent>();

        public virtual void Update(float deltaTime)
        {
        }

        /// <summary>
        /// Inicializa os parâmetros do gameobject para seu estado padrão
        /// </summary>
        public virtual void Start()
        {
        }

        /// <summary>
        /// Destroy o objeto e seus componentes
        /// </summary>
        public void Destroy()
        {
        }
    }
}