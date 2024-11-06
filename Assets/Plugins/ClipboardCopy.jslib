mergeInto(LibraryManager.library, {
	
  
  CopyToClipboard: function(text) {
  
    if (navigator && navigator.clipboard && navigator.clipboard.writeText)
    {
      navigator.clipboard.writeText(UTF8ToString(text));
    }
  }
});