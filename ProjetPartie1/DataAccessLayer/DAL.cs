using Projet.DataAccessLayer.Factories;

namespace Projet.DataAccessLayer
{
    public class DAL
    {
        private AuteurFactory? _auteurFactory = null;
        private LivreFactory? _livreFactory = null;
        private CatégorieFactory? _catégorieFactory = null;
        private EmpruntFactory? _empruntFactory = null;
        private MembreFactory? _membreFactory = null;
        private RoleFactory? _membreRoleFactory = null;
        private QuotaApiFactory? _quotaApiFactory = null;
        private LoginFactory? _loginFactory = null;

        public static string ConnectionString
        {
            get
            {
                var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
                return config.GetSection("ConnectionStrings").GetSection("Default").Value ?? string.Empty;
            }
        }
        public MembreFactory MembreFactory
        {
            get
            {
                if (_membreFactory == null)
                {
                    _membreFactory = new MembreFactory();
                }
                return _membreFactory;
            }
        }
        public EmpruntFactory EmpruntFactory
        {
            get
            {
                if (_empruntFactory == null)
                {
                    _empruntFactory = new EmpruntFactory();
                }
                return _empruntFactory;
            }
        }
        public AuteurFactory AuteurFactory
        {
            get
            {
                if(_auteurFactory == null)
                {
                    _auteurFactory= new AuteurFactory();
                }
                return _auteurFactory;
            }
        }
        public LivreFactory LivreFactory
        {
            get
            {
                if (_livreFactory == null)
                {
                    _livreFactory = new LivreFactory();
                }
                return _livreFactory;
            }
        }
        public CatégorieFactory CatégorieFactory
        {
            get
            {
                if (_catégorieFactory == null)
                {
                    _catégorieFactory = new CatégorieFactory();
                }
                return _catégorieFactory;
            }
        }

        public RoleFactory MembreRoleFactory
        {
            get
            {
                if (_membreRoleFactory == null)
                {
                    _membreRoleFactory = new RoleFactory();
                }

                return _membreRoleFactory;
            }
        }

        public QuotaApiFactory QuotaApiFactory
        {
            get
            {
                if (_quotaApiFactory == null)
                {
                    _quotaApiFactory = new QuotaApiFactory();
                }

                return _quotaApiFactory;
            }
        }

        public LoginFactory LoginFactory
        {
            get
            {
                if (_loginFactory == null)
                {
                    _loginFactory = new LoginFactory();
                }

                return _loginFactory;
            }
        }
    }
}
