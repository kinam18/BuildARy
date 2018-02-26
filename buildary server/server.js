var express = require('express');
var app = express();
var server = require('http').createServer(app);
var io=require('socket.io').listen(server);
var MongoClient = require('mongodb').MongoClient; 
var assert = require('assert');
var mongourl = 'mongodb://s1141002:159753@ds123658.mlab.com:23658/buildary';

app.set('port',process.env.PORT || 8099);

io.on("connection", function(socket){
	var currentUser;
	socket.on("USER_CONNECT",function(){
		console.log("User connected");
		socket.emit("USER_CONNECTED",{message:'test'});
	});
	socket.on("LOGIN",function(email){
		console.log("User login email:"+email.email);
		MongoClient.connect(mongourl, function(err, db) {
			assert.equal(err,null);
			console.log('Connected to MongoDB\n');
			db.collection('users').
				findOne({email: email.email},function(err,doc) {
					assert.equal(err,null);
					if(doc==null){
						var new_user={};
						new_user['email']=email.email;
						new_user['rank_score']=0;
						new_user['previous_work']=[];
						db.collection('users').insertOne(new_user,function(err,result) {
							assert.equal(err,null);
							console.log("Insert was successful!");
							doc=new_user;
						});
					}
					setTimeout(function () {
					db.close();
					console.log(doc);
					console.log('Disconnected from MongoDB\n');
					currentUser={
						email:doc.email,
						rank_score:doc.rank_score,
						previous_work:doc.previous_work
					}
					socket.emit("LOGIN",currentUser);
				},2000)
				});
		});
	});
	socket.on("GENERATOR",function(category){
		console.log("Generate word with category:"+category.category);
		MongoClient.connect(mongourl, function(err, db) {
			assert.equal(err,null);
			console.log('Connected to MongoDB\n');
			new_data={};
			db.collection('vocabulary').aggregate([
				{$match:{"category":category.category,"difficulty":"easy"}},
				{ $sample: { size: 1 } }],function(err,doc) {
					console.log("random easy success:"+doc[0].vocabulary);
					new_data['easy']=doc[0].vocabulary;
				});
			db.collection('vocabulary').aggregate([
					{$match:{"category":category.category,"difficulty":"medium"}},
					{ $sample: { size: 1 } }],function(err,doc) {
						console.log("random medium success:"+doc[0].vocabulary);
						new_data['medium']=doc[0].vocabulary;
					});
			db.collection('vocabulary').aggregate([
						{$match:{"category":category.category,"difficulty":"difficult"}},
						{ $sample: { size: 1 } }],function(err,doc) {
							console.log("random difficult success:"+doc[0].vocabulary);
							new_data['difficult']=doc[0].vocabulary;
						});
			setTimeout(function () {
			console.log(new_data);
			socket.emit("GENERATOR",new_data);
			},2000)
		});
	});
});

server.listen(app.get('port'),function(){
	console.log("---Server Running---");
}); 
