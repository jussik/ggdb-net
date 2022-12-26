function registerDrop(elem, component) {
    elem.addEventListener("drop", ev => {
        ev.preventDefault();
        if (!ev.dataTransfer)
            return;
        
        const file = ev.dataTransfer.files[0];
        if (!file || file.name !== 'galaxy-2.0.db')
            return;
        
        const r = new FileReader();
        r.onload = () => component.invokeMethodAsync('LoadDbData', new Uint8Array(r.result));
        r.readAsArrayBuffer(file);
    });
    elem.addEventListener("dragover", ev => ev.preventDefault());
}