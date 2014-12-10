using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TFIDF.Model;
using TFIDF.Service;
using System.Linq;
using System;

namespace TFIDF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private IStemmerService _stemmerService;
        private IDocumentsReaderService _documentsReaderService;
        private IKeywordsService _keywordService;
        private IFileDialogService _fileDialogService;
        private IWordNetService _wordNetService;

        private SearchEngine _searchEngine = new SearchEngine();

        private string _keywordsPath = Config.KEYWORDS_FILE;
        private string _documentPath = Config.DOCUMENTS_FILE;

        #endregion

        #region Properties

        public const string QueryPropertyName = "Query";
        public string _query = string.Empty;

        public string Query
        {
            get
            {
                return _query;
            }

            set
            {
                if (_query == value)
                {
                    return;
                }

                _query = value;
                RaisePropertyChanged(QueryPropertyName);
            }
        }

        public const string KeywordsPropertyName = "Keywords";
        private ObservableCollection<Keyword> _keywords = new ObservableCollection<Keyword>();

        public ObservableCollection<Keyword> Keywords
        {
            get
            {
                return _keywords;
            }

            set
            {
                if (_keywords == value)
                {
                    return;
                }

                _keywords = value;
                RaisePropertyChanged(KeywordsPropertyName);
            }
        }

        public const string DocumentsPropertyName = "Documents";
        private ObservableCollection<Document> _documents = new ObservableCollection<Document>();

        public ObservableCollection<Document> Documents
        {
            get
            {
                return _documents;
            }

            set
            {
                if (_documents == value)
                {
                    return;
                }

                _documents = value;
                RaisePropertyChanged(DocumentsPropertyName);
            }
        }

     
        public const string ResultPropertyName = "Result";
        private List<KeyValuePair<Document, double>> _result = new List<KeyValuePair<Document, double>>();

        public List<KeyValuePair<Document, double>> Result
        {
            get
            {
                return _result;
            }

            set
            {
                if (_result == value)
                {
                    return;
                }

                _result = value;
                RaisePropertyChanged(ResultPropertyName);
            }
        }

        #endregion

        #region Ctor

        public MainViewModel(IStemmerService stemmerService,
            IDocumentsReaderService documentsReaderService, 
            IKeywordsService keywordService,
            IFileDialogService fileDialogService,
            IWordNetService wordNetService)
        {
            _stemmerService = stemmerService;
            _documentsReaderService = documentsReaderService;
            _keywordService = keywordService;
            _fileDialogService = fileDialogService;
            _wordNetService = wordNetService;

            WordNetService w = new WordNetService();
            List<string> synonyms = w.GetSynonyms("easy");
            
            Init();
        }

        void Init()
        {
            _documentsReaderService.GetData(_documentPath,
                (item, error) =>
                {
                    if (error == null)
                    {
                        Documents = new ObservableCollection<Document>(item);
                        _searchEngine.Documents = item;
                    }
                }
            );

            _keywordService.GetData(_keywordsPath,
                (item, error) =>
                {
                    if (error == null)
                    {
                        Keywords = new ObservableCollection<Keyword>(item);
                        _searchEngine.Keywords = item;
                    }
                }
            );           
        }

        #endregion // Ctor

        #region Commands

        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ??
                    (
                    _searchCommand = new RelayCommand
                        (
                            () =>
                            {
                                string text = Query;
                                text = _stemmerService.StemText(text);
                                Document document = new Document(Query, text);

                                Dictionary<Document, double> a = _searchEngine.CalculateSimilarity(document);
                                Result = a.OrderByDescending(x => x.Value).ToList();
                                RaisePropertyChanged(ResultPropertyName);
                                
                            },
                            () =>
                            {
                                return true;
                            }
                        )
                    );
            }
        }

        private RelayCommand _openKeywordsCommand;
        public ICommand OpenKeywordsCommand
        {
            get
            {
                return _openKeywordsCommand ??
                    (
                    _openKeywordsCommand = new RelayCommand
                        (
                            () =>
                            {
                                _keywordsPath = _fileDialogService.ShowFileDialog();
                                Init();
                            },                            
                            () =>
                            {
                                return true;
                            }
                        )
                    );
            }
        }

        private RelayCommand _openDocumentsCommand;
        public ICommand OpenDocumentsCommand
        {
            get
            {
                return _openDocumentsCommand ??
                    (
                    _openDocumentsCommand = new RelayCommand
                        (
                            () =>
                            {
                                _documentPath = _fileDialogService.ShowFileDialog();
                                Init();
                            },
                            () =>
                            {
                                return true;
                            }
                        )
                    );
            }
        }

        #endregion



    }
}