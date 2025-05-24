using System.IO;
using UnityEngine;

namespace CustomUtility
{
    namespace IO
    {
        /// <summary>
        /// CSV 구조를 위한 기본 클래스.
        /// CSV 파일 경로와 구분 기호를 처리하기 위한 공통 속성과 메서드를 제공
        /// 
        /// ## 기능:
        /// - CsvTable과 CsvDictionary를 위한 공통 인터페이스를 제공
        /// - CSV 데이터의 파일 경로와 구분 기호 설정을 처리
        /// </summary>
        public class Csv
        {
            /// <summary>
            /// .csv 확장자까지 작성 필요
            /// Assets 내부에 있는 폴더 명/파일 명.csv
            /// </summary>
            [SerializeField] private string _filePath;
            public string FilePath => Path.Combine(CsvReader.BasePath, _filePath);

            /// <summary>
            /// 구분 기호
            /// </summary>
            [field: SerializeField] public char SplitSymbol { get; private set; } = ',';

            protected Csv(string path, char splitSymbol)
            {
                _filePath = path;
                SplitSymbol = splitSymbol;
            }
        }
    }
}