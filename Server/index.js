const app = require('express')();
const PORT = 9999;

const server = app.listen(PORT, () => console.log(`Hello, World! On localhost:${PORT}`));

