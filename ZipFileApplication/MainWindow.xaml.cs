using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
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

            //カッコ内(デフォルトパス)が空白の場合trueを返す OR デフォルトパスが存在すればtrue
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
            string sourceDirectory = DirectoryPath.Text;            //圧縮するファイルが入っているフォルダのパスをsouceDirectoryに格納
            var dlg = new Microsoft.Win32.SaveFileDialog            //圧縮したファイルを保存するフォルダを指定するダイアログボックスの生成
            {
                Title = "ZIPファイルの保存場所を指定してくれよな！",
                Filter = "ZIPファイル|*.zip",
                InitialDirectory = Path.GetDirectoryName(sourceDirectory),
            };

            // 
            if (dlg.ShowDialog() != true) return;

            //パスワード自動生成
            string password = System.Web.Security.Membership.GeneratePassword(8, 0);
            ResultText.Text = ($"添付ファイル名：{Path.GetFileName(dlg.FileName)}\r\n\r\nパスワード：{password}");                    //ファイル名とパスワードのテキストメッセージ表示

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
                Password = password         //パスワードを代入
            };

            //ZIPファイルを作成
            fastZip.CreateZip(zipFileDirectoryAndName, sourceDirectory, recurse, null, null);
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            //// 圧縮したいファイルが存在するフォルダを指定するためのダイアログボックスの生成
            //var folderBrowserDialog = new Forms.FolderBrowserDialog
            //{
            //    // ダイアログボックスの説明文
            //    Description = "Zipにしたいファイル・フォルダが入っているフォルダーを選択してくれよな！"
            //};
            //// ダイアログボックスのOKボタンが押されたら
            //if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    // 選択されたフォルダーパスをフォルダパスのテキストボックスに入力
            //    Settings.Default.SourcePath = DirectoryPath.Text = folderBrowserDialog.SelectedPath;
            //    Settings.Default.Save();
            //}

            var fileContent = string.Empty;
            var filePath = string.Empty;

            //using (Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog())
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            {
                openFileDialog.InitialDirectory = DirectoryPath.Text;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Zipにしたいファイル・フォルダが入っているフォルダーを選択してくれよな！";
                openFileDialog.Multiselect = true;

                // ファイルダイアログのOKボタンが押されたときの処理
                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.InitialDirectory;

                    //Read the contents of the file into a stream
                    /*var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }*/
                }
            }

            Forms.MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);

        }
    }
}

