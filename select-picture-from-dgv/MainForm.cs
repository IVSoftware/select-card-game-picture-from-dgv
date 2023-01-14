using System.ComponentModel;

namespace select_picture_from_dgv
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _imageBase = $"{GetType().Namespace}.Images.boardgamePack_v2.PNG.Cards";
            pictureBoxCard.Image = getCardImage(Value.Back);
        }
        private readonly string _imageBase;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridViewCards.AllowUserToAddRows = false;
            dataGridViewCards.RowHeadersVisible = false;
            dataGridViewCards.DataSource = Cards;
            #region F O R M A T    C O L U M N S
            Cards.Add(new Card()); // <- Auto generate columns
            dataGridViewCards.Columns["Value"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCards.Columns["Suite"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            Cards.Clear();
            #endregion F O R M A T    C O L U M N S

            // Add a few cards
            Cards.Add(new Card { Value = Value.Ten, Suite = Suite.Diamonds });
            Cards.Add(new Card { Value = Value.Jack, Suite = Suite.Clubs });
            Cards.Add(new Card { Value = Value.Queen, Suite = Suite.Hearts });
            Cards.Add(new Card { Value = Value.King, Suite = Suite.Spades });
            Cards.Add(new Card { Value = Value.Ace, Suite = Suite.Diamonds });

            dataGridViewCards.ClearSelection();
            dataGridViewCards.CellMouseClick += onCellMouseClick;
        }

        private void onCellMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            if((e.RowIndex != -1) && (e.RowIndex < Cards.Count)) 
            {
                Card card = Cards[e.RowIndex];
                pictureBoxCard.Image = getCardImage(card);
            }
        }

        BindingList<Card> Cards = new BindingList<Card>();
        private Image getCardImage(Value value = Value.Joker, Suite? suite = null)
        {
            switch (value)
            {
                case Value.Ace:
                    return localImageFromResourceName($"{_imageBase}.card{suite}A.png");
                case Value.Jack:
                    return localImageFromResourceName($"{_imageBase}.card{suite}J.png");
                case Value.Queen:
                    return localImageFromResourceName($"{_imageBase}.card{suite}Q.png");
                case Value.King:
                    return localImageFromResourceName($"{_imageBase}.card{suite}K.png");
                case Value.Joker:
                    return localImageFromResourceName($"{_imageBase}.cardJoker.png");
                case Value.Back:
                    return localImageFromResourceName($"{_imageBase}.cardBack_green3.png");
                default:
                    return localImageFromResourceName($"{_imageBase}.card{suite}{(int)value}.png");
            }
            Image localImageFromResourceName(string resource)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resource)!)
                {
                    return new Bitmap(stream);
                }
            }
        }
        private Image getCardImage(Card card) => getCardImage(card.Value, card.Suite);
    }
    enum Value { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Joker, Back }
    enum Suite { Clubs, Diamonds, Hearts, Spades, }
    class Card
    {
        // Internal set makes the cell read only
        public Value Value { get; internal set; }
        public Suite Suite { get; internal set; }
    }
}