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
        BindingList<Card> Cards = new BindingList<Card>();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridViewCards.AllowUserToAddRows = false;
            dataGridViewCards.RowHeadersVisible = false;
            dataGridViewCards.DataSource = Cards;
            #region F O R M A T    C O L U M N S
            Cards.Add(new Card()); // <- Auto generate columns
            dataGridViewCards.Columns["Value"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCards.Columns["Suit"].AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill;
            Cards.Clear();
            #endregion F O R M A T    C O L U M N S

            // Add a few cards
            Cards.Add(new Card { Value = Value.Ten, Suit = Suit.Diamonds });
            Cards.Add(new Card { Value = Value.Jack, Suit = Suit.Clubs });
            Cards.Add(new Card { Value = Value.Queen, Suit = Suit.Hearts });
            Cards.Add(new Card { Value = Value.King, Suit = Suit.Spades });
            Cards.Add(new Card { Value = Value.Ace, Suit = Suit.Diamonds });

            // Subscribe to the event when selection changes.
            dataGridViewCards.ClearSelection();
            dataGridViewCards.SelectionChanged += onSelectionChanged;
        }

        private void onSelectionChanged(object? sender, EventArgs e)
        {
            if(dataGridViewCards.CurrentCell != null)
            {
                int row = dataGridViewCards.CurrentCell.RowIndex;
                if((row != -1) && (row < Cards.Count)) 
                {
                    Card card = Cards[row];
                    pictureBoxCard.Image = getCardImage(card);
                }
            }
        }
        private Image getCardImage(Value value = Value.Joker, Suit? suit = null)
        {
            switch (value)
            {
                case Value.Ace:
                    return localImageFromResourceName($"{_imageBase}.card{suit}A.png");
                case Value.Jack:
                    return localImageFromResourceName($"{_imageBase}.card{suit}J.png");
                case Value.Queen:
                    return localImageFromResourceName($"{_imageBase}.card{suit}Q.png");
                case Value.King:
                    return localImageFromResourceName($"{_imageBase}.card{suit}K.png");
                case Value.Joker:
                    return localImageFromResourceName($"{_imageBase}.cardJoker.png");
                case Value.Back:
                    return localImageFromResourceName($"{_imageBase}.cardBack_green3.png");
                default:
                    return localImageFromResourceName($"{_imageBase}.card{suit}{(int)value}.png");
            }
            Image localImageFromResourceName(string resource)
            {
                using (var stream = GetType().Assembly.GetManifestResourceStream(resource)!)
                {
                    return new Bitmap(stream);
                }
            }
        }
        private Image getCardImage(Card card) => getCardImage(card.Value, card.Suit);
    }
    enum Value { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Joker, Back }
    enum Suit { Clubs, Diamonds, Hearts, Spades, }
    class Card
    {
        // Internal set makes the cell read only
        public Value Value { get; internal set; }
        public Suit Suit { get; internal set; }
    }
}