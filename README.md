Your post states that you want to be able to update a PictureBox when rows or cells are selected in a DataGridView. Handling the `SelectionChanged` event is one way to achieve this outcome.

[![screenshot][1]][1]

***
**DataBinding**

The excellent comment by jmcilhinney suggests data binding. So how _exactly_ would you do that? Basically, you make a class to represent a row of data, for example:

    enum Value { Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Joker, Back }
    enum Suit { Clubs, Diamonds, Hearts, Spades, }
    class Card
    {
        // Internal set makes the cell read only
        public Value Value { get; internal set; }
        public Suit Suit { get; internal set; }
    }

Then you make a `BindingList<Card>` and assign it to the `DataSource` property of your DataGridView in the method that loads your main form class.

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

            // Subscribe to the event that occurs when selection changes.
            dataGridViewCards.ClearSelection();
            dataGridViewCards.SelectionChanged += onSelectionChanged;
        }
        .
        .
        .
    }

***
**Load Images**

The other thing you need is some reliable way to locate and load an image. One good way is to set the `Build Action` property of your images to `Embedded resource`.

[![embed images][2]][2]

Now the image can be retrieved based on the car's value and suit.

    BindingList<Card> Cards = new BindingList<Card>();
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

***
**Credit**

The ace of diamonds image is from [Boardgame pack v2](https://opengameart.org/content/boardgame-pack) (Creative Commons License) by Kenney Vleugels.

  [1]: https://i.stack.imgur.com/tP7d0.png
  [2]: https://i.stack.imgur.com/Rqq3J.png