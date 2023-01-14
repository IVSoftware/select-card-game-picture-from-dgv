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
            foreach (var card in GetType().Assembly.GetManifestResourceNames())
            {

            }
            pictureBoxCard.Image = getCardImage(Value.Joker);
        }
        private readonly string _imageBase;
        private Image getCardImage(Value value, Suite? suite = null)
        {
            string fileName;
            fileName = $"{_imageBase}.cardJoker.png";
            using (var stream = GetType().Assembly.GetManifestResourceStream(fileName)!)
            {
                return new Bitmap(stream);
            }
        }
    }
}