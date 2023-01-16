Your post states that you want to be able to update a PictureBox with an image of a trading card when rows or cells are selected in a DataGridView. Your latest comment indicates that other suggestions haven't helped so far. So, than trying to diagnose from your code sample what isn't working what I can offer is an example of what will.

 Sometimes it's easier to test our coding assumptions by taking something that _works_ and introducing incremental changes until it _breaks_ so I would encourage you to [clone](https://github.com/IVSoftware/select-card-game-picture-from-dgv.git) my minimal sample and experiment with it.

Handling the `SelectionChanged` event is one way to achieve this outcome and since I don't have easy access a license for _**trading**_ card images, I will demonstrate using an open source library for _**playing**_ cards instead.

[![screenshot][1]][1]

**Image credit**: [Boardgame pack v2](https://opengameart.org/content/boardgame-pack) (Creative Commons License) by Kenney Vleugels.


***
**DataBinding**

The excellent comment by jmcilhinney suggests data binding. So how _exactly_ would you do that? Basically, you make a class to represent a row of data, for example:
    
    enum Value { Joker, Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Back }
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

As shown above, the data binding lets you add and remove cards from the DGV by manipulating the data in the `Cards` collection instead of having to work with the UI control directly.

***
**Handle event**


        private void onSelectionChanged(object? sender, EventArgs e)
        {
            if (dataGridViewCards.CurrentCell == null)
            {
                pictureBoxCard.Image = getCardImage(Value.Back);
            }
            else
            {
                int row = dataGridViewCards.CurrentCell.RowIndex;
                if((row != -1) && (row < Cards.Count)) 
                {
                    Card card = Cards[row];
                    pictureBoxCard.Image = getCardImage(card);
                }
            }
        }

***
**Load Images**

You will also need a reliable means to locate and load an image. One good way is to set the `Build Action` property of your images to `Embedded resource`.

[![embed images][2]][2]

Now the image can be retrieved based on the card's value and suit.

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



  [1]: https://i.stack.imgur.com/Q1rZh.png
  [2]: https://i.stack.imgur.com/Rqq3J.png