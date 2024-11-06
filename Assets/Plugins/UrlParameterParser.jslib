mergeInto(LibraryManager.library, {
	
      GetQueryParameter: function(paramName) {
        var urlParams = new URLSearchParams(location.search);
        var param = urlParams.get(UTF8ToString(paramName));
        console.log("JavaScript read param: " + param);
        var bufferSize = lengthBytesUTF8(param) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(param, buffer, bufferSize);
        return buffer;
    }
    
});