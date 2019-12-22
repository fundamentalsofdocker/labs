const express = require('express');
const app = express();
const initTracer = require('jaeger-client').initTracer;
const tracer = initializeTracer('my-sample-app');

app.listen(3000, '0.0.0.0', () => {
    console.log('Application listening at 0.0.0.0:3000');
})

app.get('/', (req, res) => {
    const span = tracer.startSpan("say-hello");
    res.send('Sample Application: Hello World!');
    span.finish()
})

const hobbies = [
    'Swimming', 'Diving', 'Jogging', 'Cooking', 'Singing'
];

app.get('/hobbies', (req, res) => {
    res.send(hobbies);
})

app.get('/status', (req, res) => {
    res.send('OK');
})

app.get('/colors', (req, res) => {
    res.send(['red', 'green', 'blue']);
})

function initializeTracer(serviceName) {
    var config = {
        serviceName: serviceName,
        sampler: {
            type: "const",
            param: 1,
        },
        reporter: {
            logSpans: true,
        },
    };
    var options = {
        tags: {
            'my-sample-app.version': '0.1.0',
        },
        logger: {
            info: function logInfo(msg) {
                console.log("INFO ", msg);
            },
            error: function logError(msg) {
                console.log("ERROR", msg);
            },
        },
    };
    return initTracer(config, options);
}