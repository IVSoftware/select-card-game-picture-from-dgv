namespace select_picture_from_dgv
{
    public partial class MainForm : Form
    {
        enum Value { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Joker }
        enum Suite{ Clubs, Diamonds, Hearts, Spades, }

        public MainForm()
        {
            InitializeComponent();
            _imageBase = $"{GetType().Namespace}.Images.boardgamePack_v2.PNG.Cards";
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            pictureBoxCard.Image = getCardImage(Value.Ace, Suite.Hearts);
        }
        private readonly string _imageBase;
        private Image getCardImage(Value value, Suite? suite = null)
        {
            switch (value)
            {
                case Value.Ace:
                    return localFromResourceName($"{_imageBase}.card{suite}A.png");
                case Value.Jack:
                    return localFromResourceName($"{_imageBase}.card{suite}J.png");
                case Value.Queen:
                    return localFromResourceName($"{_imageBase}.card{suite}Q.png");
                case Value.King:
                    return localFromResourceName($"{_imageBase}.card{suite}K.png");
                case Value.Joker:
                    return localFromResourceName($"{_imageBase}.cardJoker.png");
                default:
                    return localFromResourceName($"{_imageBase}.card{suite}{(int)value}.png");
            }
            Image localFromResourceName(string resource)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resource)!)
                {
                    return new Bitmap(stream);
                }
            }
        }
    }
}