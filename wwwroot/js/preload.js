const { contextBridge, ipcRenderer, shell } = require('electron');

console.log("preload.js loaded");

contextBridge.exposeInMainWorld('electronAPI', {
    // Simple wrappers for IPC usage
    send: (channel, data) => ipcRenderer.send(channel, data),
    receive: (channel, func) => ipcRenderer.on(channel, (event, ...args) => func(...args)),
    invoke: (channel, data) => ipcRenderer.invoke(channel, data),

    // Explicit wrapper for openDialog
    openDialog: (channel, config) => ipcRenderer.invoke(channel, config),

    sendExternalUrl: (url) => ipcRenderer.send("open-external-url", url),

    // Image selection dialog handler
    selectImage: () => {
        return new Promise((resolve) => {
            ipcRenderer.once('open-image-dialog-response', (event, filePath) => {
                resolve(filePath);
            });
            ipcRenderer.send('open-image-dialog');
        });
    }
});

console.log("ipcRenderer exposed in main world");
