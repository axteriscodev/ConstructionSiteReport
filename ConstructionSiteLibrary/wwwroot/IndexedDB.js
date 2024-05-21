

const DB_NAME = "ConstructorSideReport";
const VERSION = 1;
const READ_WRITE = "readwrite";
const READ = "readonly";
var db;


export function CheckDBSupport() {

    let exist = false;

    if (window.indexedDB) {
        exist = true;
    }
    console.log(exist);
    return exist;
}

/**
 * Metodo che restitusce i riferimenti al database se esiste, altrimenti
 * lo crea 
 * @param {any} dotnethelper
 */
export function CreateDB(dotnethelper) {
    let request = indexedDB.open(DB_NAME, VERSION);
    //se il db viene restituito scatta questo evento
    request.onsuccess = (event) => {
        db = event.target.result;
        console.log("sono dentro onsuccess ");
        dotnethelper.invokeMethodAsync('DbResultObservable', true).then(() => {
        }).catch(error => {
            console.log("Errore invocazione metodo cs: " + error);
        });
    };
    request.onupgradeneeded = (event) => {
        let store = event.currentTarget.result.createObjectStore("choices", { keyPath: "id" });
        console.log("objectStore" + store);
    };
    // evento di errore 
    request.onerror = (event) => {
        console.error(`Errore di caricamento database: ${event.target.errorCode}`);
        dotnethelper.invokeMethodAsync('DbResultObservable', false).then(() => {
        }).catch(error => {
            console.log("Errore invocazione metodo cs: " + error);
        });
        return false;
    };
}

/**
 * Metodo che restisce uno object store con i dati
 * @param {any} storeName
 * @param {any} mode
 * @returns
 */
export function GetObjectStorage(storeName, mode) {

    let trans = db.transaction(storeName, mode);
    return trans.objectStore(storeName);

}

/**
 * Metodo per inserire nuovi record in un determinato object store
 * @param {any} storeName
 * @param {any} records
 */
export function InsertRecords(storeName, records) {
    console.log(records);
    let store = GetObjectStorage(storeName, "readwrite");
    for (let i = 0; i < records.length; i++) {
        const request = store.add(records[i]);
        request.onsuccess = (event) => { console.log("inserito record") };
        request.onerror = (event) => { console.error("errore inserimento record: " + event.target.errorCode) };
    }
}

export function ModifyRecords(storeName, records) {
    let store = GetObjectStorage(storeName, READ_WRITE);
    for (let i = 0; i < records.length; i++) {
        const request = store.put(records[i]);
        request.onsuccess = (event) => { console.log("inserito record") };
        request.onerror = (event) => { console.error("errore inserimento record: " + event.target.errorCode) };
    }
}


/**
 * Metodo per selezionare tutti i record presenti da un determinato object store
 * @param {any} storeName
 * @param {any} dotnethelper
 */
export function selectRecords(storeName, dotnethelper) {
    let results = [];

    console.log("selecting...");
    let store = GetObjectStorage(storeName, "readonly");
    store.openCursor().onsuccess = (event) => {
        const cursor = event.target.result;
        if (cursor) {
            results.push(cursor.value);
            cursor.continue();
        } else {
            dotnethelper.invokeMethodAsync('DbObjectResultObservable', results).then(() => {
            }).catch(error => {
                console.log("Errore invocazione metodo cs: " + error);
            });
        }
    }
}




const DB_NAME = "ConstructorSideReport";
const VERSION = 1;
const READ_WRITE = "readwrite";
const READ = "readonly";
var db;


export function CheckDBSupport() {

    let exist = false;

    if (window.indexedDB) {
        exist = true;
    }
    console.log(exist);
    return exist;
}

/**
 * Metodo che restitusce i riferimenti al database se esiste, altrimenti
 * lo crea 
 * @param {any} dotnethelper
 */
export function CreateDB(dotnethelper) {
    let request = indexedDB.open(DB_NAME, VERSION);
    //se il db viene restituito scatta questo evento
    request.onsuccess = (event) => {
        db = event.target.result;
        console.log("sono dentro onsuccess ");
        dotnethelper.invokeMethodAsync('DbResultObservable', true).then(() => {
        }).catch(error => {
            console.log("Errore invocazione metodo cs: " + error);
        });
    };
    request.onupgradeneeded = (event) => {
        let store = event.currentTarget.result.createObjectStore("choices", { keyPath: "id" });
        console.log("objectStore" + store);
    };
    // evento di errore 
    request.onerror = (event) => {
        console.error(`Errore di caricamento database: ${event.target.errorCode}`);
        dotnethelper.invokeMethodAsync('DbResultObservable', false).then(() => {
        }).catch(error => {
            console.log("Errore invocazione metodo cs: " + error);
        });
        return false;
    };
}

/**
 * Metodo che restisce uno object store con i dati
 * @param {any} storeName
 * @param {any} mode
 * @returns
 */
export function GetObjectStorage(storeName, mode) {

    let trans = db.transaction(storeName, mode);
    return trans.objectStore(storeName);

}

/**
 * Metodo per inserire nuovi record in un determinato object store
 * @param {any} storeName
 * @param {any} records
 */
export function InsertRecords(storeName, records) {
    console.log(records);
    let store = GetObjectStorage(storeName, "readwrite");
    for (let i = 0; i < records.length; i++) {
        const request = store.add(records[i]);
        request.onsuccess = (event) => { console.log("inserito record") };
        request.onerror = (event) => { console.error("errore inserimento record: " + event.target.errorCode) };
    }
}

export function ModifyRecords(storeName, records) {
    let store = GetObjectStorage(storeName, READ_WRITE);
    for (let i = 0; i < records.length; i++) {
        const request = store.put(records[i]);
        request.onsuccess = (event) => { console.log("inserito record") };
        request.onerror = (event) => { console.error("errore inserimento record: " + event.target.errorCode) };
    }
}


/**
 * Metodo per selezionare tutti i record presenti da un determinato object store
 * @param {any} storeName
 * @param {any} dotnethelper
 */
export function selectRecords(storeName, dotnethelper) {
    let results = [];

    console.log("selecting...");
    let store = GetObjectStorage(storeName, "readonly");
    store.openCursor().onsuccess = (event) => {
        const cursor = event.target.result;
        if (cursor) {
            results.push(cursor.value);
            cursor.continue();
        } else {
            dotnethelper.invokeMethodAsync('DbObjectResultObservable', results).then(() => {
            }).catch(error => {
                console.log("Errore invocazione metodo cs: " + error);
            });
        }
    }
}




