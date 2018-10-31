using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using ZipFileApplication.Properties;
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

            if (string.IsNullOrWhiteSpace(Settings.Default.SourcePath) || !Directory.Exists(Settings.Default.SourcePath))
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                Settings.Default.SourcePath = Path.Combine(desktopPath, "ZipFileApplication");
                Settings.Default.Save();
            }

            DirectoryPath.Text = Settings.Default.SourcePath;
        }

        //Zipファイルを作成する一覧の処理
        private void ZipButton_Click(object sender, RoutedEventArgs e)
        {
            //圧縮するファイルが入っているフォルダのパスをsouceDirectoryに格納
            string sourceDirectory = DirectoryPath.Text;

            //圧縮したファイルを保存するフォルダを指定するダイアログボックスの生成
            var dlg = new SaveFileDialog
            {
                Title = "ZIPファイルの保存場所を指定してくれよな！",
                Filter = "ZIPファイル|*.zip",
                InitialDirectory = Path.GetDirectoryName(sourceDirectory),
            };
            if (dlg.ShowDialog() != true) return;

            //パスワード自動生成
            string password = System.Web.Security.Membership.GeneratePassword(8, 0);

            //ファイル名とパスワードのテキストメッセージ表示
            ResultText.Text = ($"添付ファイル名：{Path.GetFileName(dlg.FileName)}.zip\r\n\r\nパスワード：{password}");

            //サブディレクトリも圧縮するかどうかの指定
            bool recurse = true;

            //作成するZipファイル名とそれを置くフォルダを設定
            //ファイルが既に存在している場合は、上書きされる
            string zipFileDirectoryAndName = dlg.FileName;

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
                Settings.Default.SourcePath = DirectoryPath.Text = dlg.SelectedPath;
                Settings.Default.Save();
            }
        }
    }
}

