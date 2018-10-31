using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;


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
            //パスワードを自動生成し、そのパスワードが設定されたZipファイルを作成する
            string zipPass = System.Web.Security.Membership.GeneratePassword(8, 0);

            //圧縮するファイルが入っているフォルダのパス
            string sourceDirectory = DirectoryPath.Text;
            //サブディレクトリも圧縮するかどうか
            bool recurse = true;

            //作成するZipファイルの名前
            //ファイルが既に存在している場合は、上書きされる
            string zipFileDirectoryAndName = Path.Combine(sourceDirectory, Path.ChangeExtension(SetZipFileName.Text, ".zip"));

            FastZip fastZip = new FastZip();
            //trueなら空のフォルダもZipファイルに入れる。(デフォルトはfalse)
            fastZip.CreateEmptyDirectories = false;
            //ZIP64を使うか。デフォルトはDynamicで、状況に応じてZIP64を使う。
            //（大きなファイルはZIP64でしか圧縮できないが、対応していないアーカイバもある）
            fastZip.UseZip64 = UseZip64.Dynamic;
            //パスワードを設定
            fastZip.Password = zipPass;
            //ZIPファイルを作成
            fastZip.CreateZip(zipFileDirectoryAndName, sourceDirectory, recurse, null, null);

        }

        private void ZipPass(object sender, TextCompositionEventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // フォルダー参照ダイアログのインスタンスを生成
            var dlg = new Forms.FolderBrowserDialog();

            // ボックスの説明文
            dlg.Description = "Zipにしたいファイル・フォルダが入っているフォルダーを選択してくれよな！";

            // ダイアログボックスを表示
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたフォルダーパスをフォルダパスのテキストボックスに入力
                DirectoryPath.Text = dlg.SelectedPath;
            }
        }


    }
}

