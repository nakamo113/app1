// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;


// Console.WriteLine("Hello, World!");
// double型で計算すると少数が変になってしまったが，decimal型に変更するとうまくいった．
/* 参考サイト：StreamReaderの使い方　https://www.sejuku.net/blog/85579　
 * csvファイルの列毎読み込み　 http://csharp30matome.seesaa.net/article/132294700.html
 * 特定の桁で丸める https://www.sejuku.net/blog/104089#index_id3
*/
namespace consoleApp1
{
    class CsvParser
    {
        // CSVファイルのパス  
        private string filePath;

        // コンストラクタ  
        public CsvParser(string path)
        {
            filePath = path;
        }

        // foreach可能なメソッド  
        public IEnumerable<string> GetColumns(int idx)
        {
            // CSV Parserを使用( TextFieldParserは「using Microsoft.VisualBasic.FileIO」と参照設定が必要 )  
            using (var parser = new TextFieldParser(filePath, Encoding.GetEncoding("utf-8")))
            {
                parser.ReadLine(); //ヘッダー削除
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(","); // 区切り文字はコンマ＝CSV形式  

                while (!parser.EndOfData)
                {
                    // 1行読み込み  
                    string[] row = parser.ReadFields();
                    // N列目のカラムを返す  
                    yield return row[idx];
                }
            }
        }

    }

    class CsvWriter
    {
        private static void OutputCSV<T>(List<T> items, string fileName)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }
            // リストの先頭の要素からプロパティ名をすべて取り出し、カンマ区切りで連結する
            string headerLine = string.Join(",", items[0].GetType().GetProperties().Select(p => p.Name));

            // リストの全要素のプロパティの値をすべて取り出し、カンマ区切りで連結する
            IEnumerable<string> dataLines = from item in items
                                            let dataLine = string.Join(",", item.GetType().GetProperties().Select(p => p.GetValue(item)))
                                            select dataLine;

            // ヘッダー行とデータ行を連結
            List<string> csvData = new List<string>()
              {
                headerLine,
              };
            csvData.AddRange(dataLines);

            // CSV出力
            using StreamWriter sw = new(@$"C:\test\{fileName}.csv", false, Encoding.UTF8);
            csvData.ForEach(line =>
            {
                sw.WriteLine(line);
            });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // CSVファイル名を指定  
            var csv = new CsvParser("C_05_kyotanabedata.csv");

            int i = 0;
            string[] srHigh = new string[365]; //string型の気温データを格納する配列
            // CSVファイルの2列目(最高気温)を列挙し，decimal型の変数に格納
            foreach (string col in csv.GetColumns(1))
            {
                //Console.WriteLine(col); //コンソールに出力
                srHigh[i] = col;
                i++;
            }
            decimal[] decHigh = { 0 };
            decHigh = srHigh.Select(decimal.Parse).ToArray();// 気温のデータをstring型からdecimal型に変換
            //for (int j = 0; j < decHigh.Length; j++)
            //{
            //    Console.WriteLine(decHigh[j]); //変換できていることを確認
            //}

            //各月に配列を分ける
            decimal[] jan = new decimal[31];
            decimal[] feb = new decimal[28];
            decimal[] mar = new decimal[31];
            decimal[] apr = new decimal[30];
            decimal[] may = new decimal[31];
            decimal[] jun = new decimal[30];
            decimal[] jly = new decimal[31];
            decimal[] aug = new decimal[31];
            decimal[] sep = new decimal[30];
            decimal[] oct = new decimal[31];
            decimal[] nov = new decimal[30];
            decimal[] dec = new decimal[31];
            int sum = 0;
            Array.Copy(decHigh, sum, jan, 0, jan.Length); //decHighの0番目からコピーして，janの0番目から31個分配置
            Array.Copy(decHigh, sum+=jan.Length, feb, 0, feb.Length);
            Array.Copy(decHigh, sum+=feb.Length, mar, 0, mar.Length);
            Array.Copy(decHigh, sum += mar.Length, apr, 0, apr.Length);
            Array.Copy(decHigh, sum += apr.Length, may, 0, may.Length);
            Array.Copy(decHigh, sum += may.Length, jun, 0, jun.Length);
            Array.Copy(decHigh, sum += jun.Length, jly, 0, jly.Length);
            Array.Copy(decHigh, sum += jly.Length, aug, 0, aug.Length);
            Array.Copy(decHigh, sum += aug.Length, sep, 0, sep.Length);
            Array.Copy(decHigh, sum += sep.Length, oct, 0, oct.Length);
            Array.Copy(decHigh, sum += oct.Length, nov, 0, nov.Length);
            Array.Copy(decHigh, sum += nov.Length, dec, 0, dec.Length);
            //for (int j = 0; j < mar.Length; j++)
            //{
            //    Console.WriteLine(mar[j]); //コピーできていることを確認
            //}
            //平均を計算し，配列に格納
            decimal[] ave_h = new decimal[12];
            ave_h[0] = jan.Average();
            ave_h[1] = feb.Average();
            ave_h[2] = mar.Average();
            ave_h[3] = apr.Average();
            ave_h[4] = may.Average();
            ave_h[5] = jun.Average();
            ave_h[6] = jly.Average();
            ave_h[7] = aug.Average();
            ave_h[8] = sep.Average();
            ave_h[9] = oct.Average();
            ave_h[10] = nov.Average();
            ave_h[11] = dec.Average();

            //最低気温についても同様の処理を行う
            i = 0;
            string[] srLow = new string[365]; //string型の気温データを格納する配列
            // CSVファイルの3列目(最低気温)を列挙し，decimal型の変数に格納
            foreach (string col in csv.GetColumns(2))
            {
                //Console.WriteLine(col); //コンソールに出力
                srLow[i] = col;
                i++;
            }
            decimal[] decLow = { 0 };
            decLow = srLow.Select(decimal.Parse).ToArray();// 気温のデータをstring型からdecimal型に変換
            //for (int j = 0; j < decHigh.Length; j++)
            //{
            //    Console.WriteLine(decHigh[j]); //変換できていることを確認
            //}

            sum = 0;
            Array.Copy(decLow, sum, jan, 0, jan.Length); //decLowの0番目からコピーして，janの0番目から31個分配置
            Array.Copy(decLow, sum += jan.Length, feb, 0, feb.Length);
            Array.Copy(decLow, sum += feb.Length, mar, 0, mar.Length);
            Array.Copy(decLow, sum += mar.Length, apr, 0, apr.Length);
            Array.Copy(decLow, sum += apr.Length, may, 0, may.Length);
            Array.Copy(decLow, sum += may.Length, jun, 0, jun.Length);
            Array.Copy(decLow, sum += jun.Length, jly, 0, jly.Length);
            Array.Copy(decLow, sum += jly.Length, aug, 0, aug.Length);
            Array.Copy(decLow, sum += aug.Length, sep, 0, sep.Length);
            Array.Copy(decLow, sum += sep.Length, oct, 0, oct.Length);
            Array.Copy(decLow, sum += oct.Length, nov, 0, nov.Length);
            Array.Copy(decLow, sum += nov.Length, dec, 0, dec.Length);
            //平均を計算し，配列に格納
            decimal[] ave_l = new decimal[12];
            ave_l[0] = jan.Average();
            ave_l[1] = feb.Average();
            ave_l[2] = mar.Average();
            ave_l[3] = apr.Average();
            ave_l[4] = may.Average();
            ave_l[5] = jun.Average();
            ave_l[6] = jly.Average();
            ave_l[7] = aug.Average();
            ave_l[8] = sep.Average();
            ave_l[9] = oct.Average();
            ave_l[10] = nov.Average();
            ave_l[11] = dec.Average();

            //Console.WriteLine(ave_l[0]);

            //書き込むCSVファイルを作成
            using (FileStream fs = File.Create("./app1out.csv")) ;
            {

                // CSVファイルに出力する
                Encoding enc = Encoding.UTF8;
                //上書き書き込み
                using (StreamWriter writer = new StreamWriter("./app1out.csv", false, enc))
                {
                    //ヘッダー作成
                    writer.WriteLine("月,最高気温平均(℃),最低気温平均(℃)");

                    for (i=1;i<=12;i++)
                    {
                        // 少数第四位で四捨五入
                        writer.WriteLine(i + "月," + Math.Round(ave_h[i - 1], 3, MidpointRounding.AwayFromZero) + "," + Math.Round(ave_l[i - 1], 3, MidpointRounding.AwayFromZero));
                    }
                    Console.WriteLine("csv出力完了");
                }

            }
        }

    }
}