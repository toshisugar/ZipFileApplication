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

        //Zipファイルを作成する一覧の処理
        private void ZipButton_Click(object sender, RoutedEventArgs e)
        {
            //パスワード自動生成
            string password = System.Web.Security.Membership.GeneratePassword(8, 0);

            //ファイル名とパスワードのテキストメッセージ表示
            ResultText.Text = ($"添付ファイル名：{SetZipFileName.Text}.zip\r\n\r\nパスワード：{password}");

            //圧縮するファイルが入っているフォルダのパスをsouceDirectoryに格納
            string sourceDirectory = DirectoryPath.Text;

            //サブディレクトリも圧縮するかどうかの指定
            bool recurse = true;

            //圧縮したファイルを保存するフォルダを指定するダイアログボックスの生成
            var SaveFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var dlg = new Forms.FolderBrowserDialog
            {
                // ダイアログボックスの説明文
                Description = "保存先フォルダを指定してくれよな！"
            };
            // ダイアログボックスのOKボタンが押されたら
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
            // ダイアログボックスのOKボタンが押されたら
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをフォルダパスのテキストボックスに入力
                DirectoryPath.Text = dlg.SelectedPath;
            }
        }
    }
}

