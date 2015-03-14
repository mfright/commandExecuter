using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javaExecuter
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // ウィンドウを閉じるタイマーを始動
            timer1.Start();

            //　コマンドを読み込んで実行するスレッドを実行
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(readAndRun));

            t.Start();

            

            //Application.Exit();

        }

        private void readAndRun()
        {
            // コマンドの書かれたファイルを読み込む
            String filePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\settings.ini";

            System.IO.StreamReader sr = new System.IO.StreamReader(
                filePath,
                System.Text.Encoding.GetEncoding("shift_jis"));
            //内容を一行ずつ読み込んで実行。
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                Console.WriteLine(line);
                run(line);

            }
            //閉じる
            sr.Close();
        }



        private void run(string com)
        {
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            //出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            //ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;
            //コマンドラインを指定（"/c"は実行後閉じるために必要）
            p.StartInfo.Arguments = @"/c "+com;

            //起動
            p.Start();

            //出力を読み取る
            string results = p.StandardOutput.ReadToEnd();

            //プロセス終了まで待機する
            //WaitForExitはReadToEndの後である必要がある
            //(親プロセス、子プロセスでブロック防止のため)
            p.WaitForExit();
            p.Close();

            //出力された結果を表示
            Console.WriteLine(results);
        }

        // タイマーが打った時
        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
