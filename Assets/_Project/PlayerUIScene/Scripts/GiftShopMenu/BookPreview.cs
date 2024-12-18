using System.Collections;
using System.IO;
using Core.UI;
using Metaverse.GiftShop.PurchaseItem;
using Paroxe.PdfRenderer;
using ProjectConfig.General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerUIScene.GiftShopMenu
{
    public sealed class BookPreview : UIController
    {
        private const int DefaultPageIndex = 0;
        private const int StepValue = 1;

        [SerializeField] private PDFViewer _pdfViewer;
        [SerializeField] private DownloadProgressWindow _downloadProgressWindow;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _previousPageButton;
        [SerializeField] private Button _skipToEndButton;
        [SerializeField] private Button _skipToTopButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _pageCountText;

        private BookItemInformation _currentBookInfo;
        private bool _isBookLoaded;

        private void OnEnable()
        {
            _pdfViewer.OnCurrentPageChanged += UpdatePageCounter;
            _pdfViewer.OnDocumentLoaded += OnBookLoaded;
            _downloadProgressWindow.OnLoadBook.AddListener(LoadBook);
            _nextPageButton.onClick.AddListener(OnForwardButtonClick);
            _previousPageButton.onClick.AddListener(OnBackButtonClick);
            _skipToEndButton.onClick.AddListener(OnSkipToEndButtonClick);
            _skipToTopButton.onClick.AddListener(OnSkipToTopButtonClick);
            _closeButton.onClick.AddListener(OnBackToTopButtonClick);
        }

        private void OnDisable()
        {
            _pdfViewer.OnCurrentPageChanged -= UpdatePageCounter;
            _pdfViewer.OnDocumentLoaded -= OnBookLoaded;
            _downloadProgressWindow.OnLoadBook.RemoveListener(LoadBook);
            _nextPageButton.onClick.RemoveListener(OnForwardButtonClick);
            _previousPageButton.onClick.RemoveListener(OnBackButtonClick);
            _skipToEndButton.onClick.RemoveListener(OnSkipToEndButtonClick);
            _skipToTopButton.onClick.RemoveListener(OnSkipToTopButtonClick);
            _closeButton.onClick.RemoveListener(OnBackToTopButtonClick);
        }

        public void ShowBookPreview(BookItemInformation info)
        {
            _currentBookInfo = info;
            LoadBook();
            Show();
        }

        private void UpdatePageCounter(PDFViewer sender, int oldPageIndex, int newPageIndex)
        {
            _pageCountText.text = (newPageIndex + StepValue).ToString();
        }

        private void OnBookLoaded(PDFViewer sender, PDFDocument document)
        {
            _isBookLoaded = true;
        }

        private void OnForwardButtonClick()
        {
            _pdfViewer.GoToNextPage();
        }

        private void OnBackButtonClick()
        {
            _pdfViewer.GoToPreviousPage();
        }

        private void OnSkipToEndButtonClick()
        {
            _pdfViewer.GoToPage(_pdfViewer.PageCount);
        }

        private void OnSkipToTopButtonClick()
        {
            _pdfViewer.GoToPage(DefaultPageIndex);
        }

        private void OnBackToTopButtonClick()
        {
            Hide();
        }

        private void LoadBook()
        {
            if (_isBookLoaded)
            {
                _pdfViewer.GoToPage(DefaultPageIndex);
                return;
            }

            _downloadProgressWindow.Show();
            _pdfViewer.LoadDocumentFromWeb(Path.Combine(GeneralSettings.ContentFolderUrl, _currentBookInfo.BookPdfName),
                _currentBookInfo.BookPassword);
        }
    }
}