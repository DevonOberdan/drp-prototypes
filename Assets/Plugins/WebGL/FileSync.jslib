mergeInto(LibraryManager.library, {
  SyncFiles: function () {
    if (typeof FS === 'undefined') {
      console.warn("SyncFiles: FS is undefined");
      return;
    }

    try {
      FS.syncfs(false, function (err) {
        if (err) {
          console.error("SyncFiles error:", err);
        } else {
          console.log("SyncFiles: filesystem synced successfully.");
        }
      });
    } catch (e) {
      console.error("SyncFiles exception:", e);
    }
  }
});
