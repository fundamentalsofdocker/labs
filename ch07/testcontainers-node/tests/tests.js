const request = require("request");
const path = require('path');
const dns = require('dns');
const os = require('os');
const { GenericContainer } = require("testcontainers");

(async () => {
    console.log("Starting Postgres...");
    const localPath = path.resolve(__dirname, "../database");
    const dbContainer = await new GenericContainer("postgres")
    .withName("postgres")
    .withExposedPorts(5432)
    .withEnv("POSTGRES_USER", "dbuser")
    .withEnv("POSTGRES_DB", "sample-db")
    .withBindMount(localPath, "/docker-entrypoint-initdb.d")
    .withTmpFs({ "/temp_pgdata": "rw,noexec,nosuid,size=65536k" })
    .start();
    
    const dbPort = dbContainer.getMappedPort(5432);
    console.log("Postgres listening at host port: %d", dbPort);

    const myIP4 = await lookupPromise();
    console.log("My IP4 is: %s", myIP4);

    console.log("Building API container image...");
    const buildContext = path.resolve(__dirname, "../api");
    const apiContainer = await GenericContainer
        .fromDockerfile(buildContext)
        .build();
    console.log("Image name is: %s:%s", apiContainer.image, apiContainer.tag);

    console.log("Starting API container...");
    const startedApiContainer = await apiContainer
        .withName("api")
        .withExposedPorts(3000)
        .withEnv("DB_HOST", myIP4)
        .withEnv("DB_PORT", dbPort)
        .start();

    const apiPort = startedApiContainer.getMappedPort(3000);
    console.log("API listening at host port %d", apiPort);
    
    const base_url = `http://localhost:${apiPort}`
    console.log("API base URL: %s", base_url);
    request.get(base_url + "/hobbies", (error, response, body) => {
        console.log("> expecting status code 200");
        if(response.statusCode != 200){
            logError(`Unexpected status code ${response.statusCode}`);
        }
        const hobbies = JSON.parse(body);
        console.log("> expecting length of hobbies == 5");
        if(hobbies.length != 5){
            logError(`${hobbies.length} != 5`);
        }
        console.log("> expecting first hobby == swimming");
        if(hobbies[0].hobby != "swimming"){
            logError(`${hobbies[0].hobby} != swimming`);
        }
        cleaningUp();
    });
    
    function logError(message){
        console.log('\x1b[31m%s\x1b[0m', `***ERR: ${message}`);
    }

    async function cleaningUp() {
        console.log("Cleaning up...");
        await startedApiContainer.stop()
        await dbContainer.stop();
    }

    async function lookupPromise(){
        return new Promise((resolve, reject) => {
            dns.lookup(os.hostname(), (err, address, family) => {
                if(err) throw reject(err);
                resolve(address);
            });
       });
    };

})();
