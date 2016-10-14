using System.Collections.Generic;
using SFMLFramework.src;
using SFML.System;

namespace SFMLFramework
{
    /// <summary>
    /// Define um objeto do game, com eventos de interação com o gameloop e propriedades de controle universais
    /// </summary>
    public class GameObject : Observable<GameObject>
    {
        #region Fields

        /// <summary>
        /// GameObject está habilitado? (somente executa Update caso esteja)
        /// </summary>
        protected bool isEnabled;

        /// <summary>
        /// Nome do objeto
        /// </summary>
        public string name;

        /// <summary>
        /// Id único do objeto
        /// </summary>
        protected int id;

        /// <summary>
        /// Posição do game object no mundo
        /// </summary>
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
            this.isEnabled = true;
        }


        /// <summary>
        /// Lista de componentes adicionados ao gameobject
        /// </summary>
        public List<IComponent> Components = new List<IComponent>();

        #endregion


        #region Public 

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="name">Nome do gane object</param>
        public GameObject(string name)
        {
            this.name = name;
            this.isEnabled = true;
        }

        /// <summary>
        /// Atualiza todos os componentes adicionados a lista de componentes
        /// </summary>
        /// <param name="deltaTime">Tempo transcorrido desde o último frame</param>
        public virtual void Update(float deltaTime)
        {
            this.Components.ForEach(c => c.Update(deltaTime));
        }

        /// <summary>
        /// Inicializa os parâmetros do gameobject para seu estado padrão
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Destroy o objeto e seus componentes
        /// </summary>
        public void Destroy() { }

        //TODO: testar....
        /// <summary>
        /// Adiciona um componente ao game object e retorna a instância do mesmo
        /// </summary>
        /// <typeparam name="T">Tipo do componente</typeparam>
        /// <returns>Componente instanciado</returns>
        public T AddComponent<T>() where T : IComponent
        {
            T t = GetComponent<T>();
            if (t == null)
            {
                t = (T)new object();
                this.Components.Add(t);
            }
            return t;
        }

        /// <summary>
        /// Obtém um componente do tipo especificado, caso exista.
        /// </summary>
        /// <typeparam name="T">Tipo do componente</typeparam>
        /// <returns>Instância do componente</returns>
        public T GetComponent<T>()
        {
            return (T)this.Components.Find(c => c.GetType().Equals(typeof(T)));
        }

        #endregion
    }
}