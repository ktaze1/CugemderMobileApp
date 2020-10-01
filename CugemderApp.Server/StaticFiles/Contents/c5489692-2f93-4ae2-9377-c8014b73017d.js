const crypto = require ('crypto');
const express = require ('express');
const http = require ('http');
const app = express();

var drones = []; //array for each drone
var colorMap = {}; //map for holding colors


function exploreMap(){
    for (var i = 1; i<200; i++){ //limit suppose to be 1000, but my pc is not strong enough
        drones.push(new Drone([{d:'n', st: i}, {d:'w', st:-10}]));
        drones.push(new Drone([{d:'n', st: i}, {d:'e', st:-10}]));
        drones.push(new Drone([{d:'s', st: i}, {d:'w', st:-10}]));
        drones.push(new Drone([{d:'s', st: i}, {d:'e', st:-10}]));
        //-10 indicates infinite, drone will go as much as it can
    }
}

function Drone (steps){
    var error = null;
    var xCoor = 0;      
    var yCoor = 0;  //x&y coordinate parameter for each drone
   
    function goNextStep (response){
        if (response.color) {//if color exists put it on colorMap
            colorMap [xCoor+ ',' +yCoor] = response.color;
        }

        if(!error && steps.length > 0){ //if there are no errors and there are ways to go
            let direction;
            if (steps[0].st > 0){ //are are steps to take
                direction = steps[0].d; //take the direction 
                steps[0].st--; //decrease the step count
                if (steps[0].st === 0) { //when drone can no longer go dequeue
                    steps = steps.slice(1, steps.length); //queue operation. slow but works
                }
            } else { //change direction
                direction = steps[0].d;
            }

            if (typeof response[direction] === 'undefined'){ // if response's direction is not defined give error.
                error = 'direction not found';
                return;
            }


            //update coordinates
            xCoor += directions[direction][0];
            yCoor += directions[direction][1];

            let token = createToken(response[direction]);
            httpRequest('http://72.14.190.248:34873/kaan/' + response[direction] + '?token=' + token);
        }
    };

    function httpRequest (url){
        http.get(url, (response) =>{
            const {statusCode} = response;
            if(statusCode !== 200){ //if status is not OK
                error = `Request failed. \nStatus code : ${statusCode}`;
                console.error(error);
                return;
            }

            let rawData = '';
            response.on('data',(chunk) =>{
                rawData += chunk;
            });
            response.on('end', () =>{
                try{
                    goNextStep(JSON.parse(rawData));
                   }
                catch(e){
                    console.log(e);
                }
            });
        }).on('error', (e)=>{
            error = `Got error: ${e.message}`;
            console.error(error);
        });
    };

    goNextStep({n:0, s:0, e:0, w:0});
};


var directions = {
    sw: [-1, -1],
    w:  [-1, 0],
    nw: [-1, 1],
    n:  [0, 1],
    ne: [1, 1],
    e: [1, 0],
    se: [1, -1],
    s: [0, -1]
};


function md5Hash (str) { //make md5 hash
    var hash = crypto.createHash('md5');
    hash.update(str);
    return hash.digest('hex');
}

function createToken (state) { //make link to request
    var seconds = Math.floor((new Date).getTime() / 1000);
    var hashStr = 'kaan|' + seconds + '|be49c83ab9926c8646ea44cb5bf6d8ad|' + state;
    return md5Hash(hashStr);
}

function handleColorRequest (req, res) { //draw map
    res.send(JSON.stringify(colorMap));
}

app.get('/colors', handleColorRequest);
app.use(express.static('public')); 

app.listen(3333); //listen on port 3333
exploreMap();