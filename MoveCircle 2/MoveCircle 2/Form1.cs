using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveCircle_2
{
    public partial class FormBallGame : Form
    {
        //クラスの共通の変数
        private Bitmap canvas; //　画面下の描画領域
        private Ball[] balls; //　ボールを管理
        private string[] kanjis; // ボールに描く漢字の配列
        private Brush[] brushes; //ボールを塗る色の配列
        private string fontName = "HG教科書体"; //　表示する漢字のフォント名
        private string correctText = "荻"; //　正解の文字：１つだけ
        private string mistakeText = "萩"; // 間違いの文字：ボールの個数分並ぶ
        private string circleText = "〇"; // 正解した場合背景の文字を〇にする
        private double nowTime = 0; //　経過時間
        private int ballCount = 5; // ボールの数
        private int randomResult = 0; // 正解の番号：0？ボールの数のいずれか

        public int i { get; private set; }

        public FormBallGame()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------------------
        //独自のメソッド
        //---------------------------------------------------------------------
        // 上のselectpictureBoxコントロールに円を描いてみる
        private void DrawCircleSelectPictureBox()
        {
            int height = selectPictureBox.Height; //　高さ
            int width = selectPictureBox.Width; //　幅

            Bitmap selectCanvas = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(selectCanvas))
            {
                //g.FillEllipse(Brushes.LightBlue, 0, 0, height, height);
                for (int i = 0; i < ballCount; i++)
                {
                    g.FillEllipse(brushes[i], i * height, 0, height, height);
                }
                selectPictureBox.Image = selectCanvas;
            }
        }

        // 下のPictureBoxに描画する(引数で色と文字を指定できるようにする)
        private void DrawMainPictureBox(Brush color, string text, bool trueFlag)
        {
            // 描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(mainPictureBox.Width, mainPictureBox.Height);
            using (Graphics g = Graphics.FromImage(canvas))
            {
                //背景に引数で指定した文字列を描画する
                g.DrawString("荻",
                    new Font("HG教科書体",
                    mainPictureBox.Height - mainPictureBox.Height / 4),
                    Brushes.Gray, 0, 0, new StringFormat());


                //mainPictueBoxに表示する
                mainPictureBox.Image = canvas;

            }
            int height = mainPictureBox.Height; //高さ
            int width = mainPictureBox.Width; //幅

            //描画先とするImageオブジェクトを作成する
            if (canvas == null)
            {
                canvas = new Bitmap(width, height);
            }
            using (Graphics g = Graphics.FromImage(canvas))
            {
                //正解用の背景色にする
                if (trueFlag)
                {
                    g.FillRectangle(Brushes.LightPink, 0, 0, width, height);
                }
                else
                {
                    g.FillRectangle(Brushes.White, 0, 0, width, height);
                }
                //背景に文字列を描画する
                g.DrawString(text,
                    new Font(fontName, height - height / 4),
                    color, 0, 0, new StringFormat());

                //mainPictureBoxに表示する
                mainPictureBox.Image = canvas;
            }
        }
        //----------------------------------------------------------------------
        //イベントハンドラ
        //----------------------------------------------------------------------

        private void FormBallGame_Load(object sender, EventArgs e)
        {
            ////DrawCircleSelectPictureBox();
            ////DrawMainPictureBox();
            //DrawCircleSelectPictureBox(); //上のPictureBoxに円を描く
            //DrawMainPictureBox(Brushes.Gray, correctText); //下のPictureBoxに円を描く
            //textHunt.Text = correctText;

            //// ボールクラスのインスタンス作成
            //balls = new Ball(mainPictureBox, canvas, Brushes.LightBlue, correctText);
            //// 位置100,100にボールを億
            //balls.PutCircle(100, 100);

            //// タイマーをスタートさせる
            //timer1.Start();
            InitGraphics();
            SetStartPosition();
        }

   

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        // 再スタートボタンが押された時、呼ばれるイベントハンドラ
        private void restartButton_Click(object sender, EventArgs e)
        {
            InitGraphics();
            SetStartPosition();
        }
        // タイマーが動いているとき、呼ばれるイベントハンドラ
        private void timer1_Tick(object sender, EventArgs e)
        {
            //balls.Move();

            //nowTime = nowTime + 0.02;
            //textTimer.Text = nowTime.ToString("0.00");
            for (int i = 0; i < ballCount; i++)
            {
                balls[i].Move();
            }

            nowTime = nowTime + 0.02;
            textTimer.Text = nowTime.ToString("0.00");
        }

        private void selectPictureBox_Click(object sender, EventArgs e)
        {

        }

        // 下のピクチャーボックスが押された時、呼ばれるイベントハンドラ
        private void mainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            //再スタートボタンが操作可能な場合は何もせずに処理完了
            if (restartButton.Enabled)
            {
                return;
            }

            setBalls(e.X, e.Y); //マウスをクリックした位置にボールをセット
        }


        //配列の初期化、画面の初期設定を行う
        private void InitGraphics()
        {
            brushes = new Brush[ballCount];
            kanjis = new string[ballCount];
            balls = new Ball[ballCount];

            //ブラシの色の設定
            //色の詳細はこちら：
            brushes[0] = Brushes.LightPink;
            brushes[1] = Brushes.LightBlue;
            brushes[2] = Brushes.LightGray;
            brushes[3] = Brushes.LightCoral;
            brushes[4] = Brushes.LightGreen;

            //上のImageのオブジェクト
            DrawCircleSelectPictureBox();
            //下のImageオブジェクト
            DrawMainPictureBox(Brushes.Gray, correctText, false);

            restartButton.Enabled = false; //再スタートボタンを操作できないようにする
            textHunt.Text = correctText;
        }
        //ボールのインスタンスの作成とランダムな位置にボールを描く
        private void SetStartPosition()
        {
            //漢字の設定
            for (int i = 0; i < ballCount; i++)
            {
                kanjis[i] = mistakeText; // 間違いの文字をセット
            }
            randomResult = new Random().Next(ballCount); //ボールの数分の乱数を取得
            kanjis[randomResult] = correctText; //正解の文字をセット

            //ランダムな位置にボールを描く
            int rndXMax = mainPictureBox.Width;
            int rndYMax = mainPictureBox.Height;

            setBalls(new Random().Next(rndXMax), new Random().Next(rndYMax));

            //タイマーをスタートさせる
            nowTime = 0;
            timer1.Start();
        }
        //引数の位置情報を利用してランダムにボールを描く
        private void setBalls(int x, int y)
        {
            int rndXMax = mainPictureBox.Width;
            int rndYMax = mainPictureBox.Height;
            int rndX;
            int rndY;

            for (int i = 0; i < ballCount; i++)
            {
                rndX = new Random(i * x).Next(rndXMax);
                rndY = new Random(i * y).Next(rndYMax);
                balls[i].DeleteCircle(); //
                balls[i].PutCircle(rndX, rndY); //新しい位置にボールを描く
            }
        }

        private void selectPictureBox_MouseClick(object sender, EventArgs e)
        {

        }

        private void selectPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            //再スタートボタンが操作可能な場合は何もせずに処理終了
            if (restartButton.Enabled)
            {
                return;
            }

            //押されたX座標で正解判定
            //<判定>押されたボタンがマウスの左ボタン？
            if (e.Button == MouseButtons.Left)
            {
                //どの円を選択したかを計算で算出(クリックしたｘ座標の位置/pictureBoxの横幅)
                int selectCircle = e.X / selectPictureBox.Height;
                if (randomResult == selectCircle) // 正解の円を選んだ
                {
                    timer1.Stop();
                    DrawMainPictureBox(Brushes.Red, circleText, true);
                    restartButton.Enabled = true; //再スタートボタンを操作可能に
                }
                else //失敗
                {
                    DrawMainPictureBox(Brushes.Red, correctText, false);
                    // 移動の割合を減少させる
                    for (int i = 0; i < ballCount; i++)
                    {
                        balls[i].pitch = balls[i].pitch - balls[i].pitch / 2;
                    }
                    nowTime = nowTime + 10; //ペナルティ
                }

            }
        }
    }
}
