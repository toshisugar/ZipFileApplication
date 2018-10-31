using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Windows;
using Forms = System.Windows.Forms;


namespace ZipFileApplication
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryPath.Text = Path.Combine(desktopPath, "ZipFileApplication");

        }

        //Zipファイルの作成を行う
        private void ZipButton_Click(object sender, RoutedEventArgs e)
        {
            //パスワードを自動生成する
            string password = System.Web.Security.Membership.GeneratePassword(8, 0);

            //圧縮するファイルが入っているフォルダのパスを指定する
            string sourceDirectory = DirectoryPath.Text;

            //サブディレクトリも圧縮するかどうかを決める
            bool recurse = true;

            //圧縮ファイルを保存するフォルダを指定するダイアログボックスの生成
            var SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var dlg = new Forms.FolderBrowserDialog
            {
                // ダイアログボックスの説明文
                Description = "保存先フォルダを指定してくれよな！"
            };
            // ダイアログボックスを表示
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスを、保存先を格納する変数に代入
                SaveFolder = dlg.SelectedPath;
            }

            //作成するZipファイル名とそれを置くフォルダを設定
            //ファイルが既に存在している場合は、上書きされる
            string zipFileDirectoryAndName = Path.Combine(SaveFolder, Path.ChangeExtension(SetZipFileName.Text, ".zip"));

            FastZip fastZip = new FastZip
            {
                //trueなら空のフォルダもZipファイルに入れる。(デフォルトはfalse)
                CreateEmptyDirectories = false,
                //ZIP64を使うか。デフォルトはDynamicで、状況に応じてZIP64を使う（大きなファイルはZIP64でしか圧縮できないが、対応していないアーカイバもある）。
                UseZip64 = UseZip64.Dynamic,
                //パスワードを設定
                Password = password
            };
            //ZIPファイルを作成
            fastZip.CreateZip(zipFileDirectoryAndName, sourceDirectory, recurse, null, null);

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // 圧縮したいファイルが存在するフォルダを指定するためのダイアログボックスの生成
            var dlg = new Forms.FolderBrowserDialog
            {
                // ダイアログボックスの説明文
                Description = "Zipにしたいファイル・フォルダが入っているフォルダーを選択してくれよな！"
            };
            // ダイアログボックスを表示
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをフォルダパスのテキストボックスに入力
                DirectoryPath.Text = dlg.SelectedPath;
            }
        }


    }
}

