mergeInto(LibraryManager.library, {
  GetEventId: function () {
    const eventId = window.localStorage.getItem('eventId');

    var bufferSize = lengthBytesUTF8(eventId) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(eventId, buffer, bufferSize);
    return buffer;
  },

  GetServiceIP: function () {
    const serviceIP = window.localStorage.getItem('serviceIP');

    var bufferSize = lengthBytesUTF8(serviceIP) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(serviceIP, buffer, bufferSize);
    return buffer;
  },

  GetServicePort: function () {
    const servicePort = window.localStorage.getItem('servicePort');

    var bufferSize = lengthBytesUTF8(servicePort) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(servicePort, buffer, bufferSize);
    return buffer;
  },

  GetPlayerName: function () {
    const playerName = window.localStorage.getItem('playerName');

    var bufferSize = lengthBytesUTF8(playerName) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(playerName, buffer, bufferSize);
    return buffer;
  },

  QuitGame: function () {
    quitGame();
  },
});
