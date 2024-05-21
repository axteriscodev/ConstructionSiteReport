const DB_NAME = "ConstructorSideReport";
const VERSION = 1;
const READ_WRITE = "readwrite";
const READ_ONLY = "readonly";
var db;

//-------------------------------------------------//
// Metodi esposti al C#

/**
 * Metodo che controlla che IndexedDB sia supportato
 * @returns true se supportato, altrimenti false
 */
export function checkDBSupport() {

    let exist = false;

    if (window.indexedDB) {
        exist = true;
    }
    console.log(exist);
    return exist;
}

/**
 * Metodo che restitusce i riferimenti al database se esiste, altrimenti
 * lo crea (object store compresi)
 * @returns true se il db è stato aperto/creato correttamente
 */
export function openDB() {
    return new Promise((resolve) => {
        let request = indexedDB.open(DB_NAME, VERSION);
        //se il db viene restituito scatta questo evento
        request.onsuccess = (event) => {
            db = event.target.result;
            resolve(true);
        };
        request.onupgradeneeded = (event) => {
            let choiceStore = event.currentTarget.result.createObjectStore("choices", { keyPath: "id" });
            let questionStore = event.currentTarget.result.createObjectStore("questions", { keyPath: "id" });
            let categoryStore = event.currentTarget.result.createObjectStore("categories", { keyPath: "id" });
            let documentStore = event.currentTarget.result.createObjectStore("documents", { keyPath: "id" });
            console.log("objectStore: " + choiceStore.name);
            console.log("objectStore: " + questionStore.name);
            console.log("objectStore: " + categoryStore.name);
            console.log("objectStore: " + documentStore.name);
        };
        // evento di errore 
        request.onerror = (event) => {
            console.error("errore OpenDB: " + event.target.errorCode);
            resolve(false);
        };
    })
}

/**
 * Metodo utilizzato per inserire più oggetti in un object store definito
 * @param {any} storeName il nome dell'objectStore
 * @param {any} records la lista di record da inserire
 * @returns il count con il numero degli oggetti inseriti
 */
export async function inserts(storeName, records) {

    let count = 0;
    let store = GetObjectStorage(storeName, READ_WRITE);
    for (let i = 0; i < records.length; i++) {
        count += await InsertRecord(store, records[i]);
    }
    console.log("count: " + count);
    return count;
}

/**
 * Metodo per selezionare tutti i record contenuti in un objectStore
 * @param {any} storeName il nome dell'objectStore
 * @returns la lista di oggetti
 */
export function selectMulti(storeName) {
    return new Promise((resolve) => {
        let results = [];
        let store = GetObjectStorage(storeName, READ_ONLY);
        store.openCursor().onsuccess = (event) => {
            const cursor = event.target.result;
            if (cursor) {
                results.push(cursor.value);
                console.log(cursor.value)
                cursor.continue();
            } else {
                resolve(results);
            }
        };
        store.openCursor().onerror = (event) => {
            console.error("errore SelectMulti: " + event.target.errorCode);
            resolve(results);
        }
    });
}

/**
 * Metodo per selezionare un record da un object store in base alla sua key (id)
 * @param {any} storeName
 * @param {any} id
 * @param {any} dotnethelper
 */
export function selectByKey(storeName, key) {
    return new Promise((resolve) => {
        let store = GetObjectStorage(storeName, READ_ONLY);
        const request = store.get(key);
        request.onsuccess = () => {
            resolve(request.result);
        };
        request.onerror = (event) => {
            console.error("errore selectByKey: " + event.target.errorCode);
            resolve(null);
        }
    });
}

export function deleteRecord(storeName, key) {
    return new Promise((resolve) => {
        let store = GetObjectStorage(storeName, READ_WRITE);
        const request = store.delete(key);
        request.onsuccess = () => {
            resolve(true);
        };
        request.onerror = (event) => {
            console.error("errore deleteRecord: " + event.target.errorCode);
            resolve(false);
        }
    })
}

//-------------------------------------------------//
// Metodi interni javascript

/**
 * Metodo che restisce uno object store con i dati
 * @param {any} storeName il nome dell objectStore
 * @param {any} mode la modalità di utilizzo (read_write/ read_only)
 * @returns l'oggetto objectStore
 */
function GetObjectStorage(storeName, mode) {

    let trans = db.transaction(storeName, mode);
    return trans.objectStore(storeName);

}

/**
 * Metodo che esegue l'inserimento di un oggetto in un object store definito
 * @param {any} storeName l'objectStore dove inserire i dati
 * @param {any} record l'oggetto da inserire
 */
function InsertRecord(store, record) {
    return new Promise((resolve) => {
        const request = store.put(record);
        request.onsuccess = () => {
            console.log("inserito record");
            resolve(1);
        };
        request.onerror = (event) => {
            console.error("errore inserimento record: " + event.target.errorCode);
            resolve(0);
        };
    });
}

async function NextKey(storeName) {
    return new Promise((resolve) => {
        let store = GetObjectStorage(storeName, READ_ONLY);
        const cursor = store.openKeyCursor(null, "prev");
        cursor.onsuccess = () => {
            const key = cursor.result.key + 1;
            resolve(key);
        }
    });
}