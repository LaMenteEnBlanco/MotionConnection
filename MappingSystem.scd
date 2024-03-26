OSCFunc.trace(true);
OSCFunc.trace(false);


(
ServerOptions.devices;
s.options.memSize = 8192 * 8;
s.options.numOutputBusChannels = 8;
s.options.numWireBufs = 64 * 10;
s.options.inDevice = "BlackHole 2ch";
s.options.blockSize = 64*8;
//s.options.outDevice = "AirPod_Lautsprecher_Agregate";
//s.options.outDevice = "MacBook Pro-Lautsprecher";
s.options.outDevice = "LG TV SSCR";
s.reboot;
)
().play

s.reboot

//Synths & Functions
(
~inSpecArray = [
	/*\distanceHands*/ ControlSpec(0.0, 1.0, \db, 0.01, 0, units: ""),
	/*\distanceElbows*/ ControlSpec(0.0, 1.0, \lin, 0.0, 0, units: ""),
	/*\averageHeight*/ ControlSpec(0.0, 2.0, \lin, 0.01, 0, units: ""),
	/*\angleShouldersHips*/ ControlSpec(0.0, 180.0, \lin, 0.01, 0, units: ""),
	/*\compressedness*/ ControlSpec(0.0, 1.0, \lin, 0.01, 0, units: ""),
	/*\angleElbowLeft*/ ControlSpec(13.0, 155.0, \lin, 0.01, 0, units: ""),
	/*\angleElbowRight*/ ControlSpec(23.0, 152.0, \lin, 0.01, 0, units: ""),
	/*\angleKneeLeft*/ ControlSpec(17.0, 130.0, \lin, 0.01, 0, units: ""),
	/*\angleKneeRight*/ ControlSpec(7.0, 125.0, \lin, 0.01, 0, units: ""),
	/*\angleShoulderLeft*/ ControlSpec(0.0, 180.0, \lin, 0.01, 0, units: ""),
	/*\angleShoulderRight*/ ControlSpec(0.0, 180.0, \lin, 0.01, 0s, units: "")
];
~inNamesDict = Dictionary[
	\distanceHands -> 0,
	\distanceElbows -> 1,
	\averageHeight -> 2,
	\angleShouldersHips -> 3,
	\compressedness -> 4,
	\angleElbowLeft -> 5,
	\angleElbowRight -> 6,
	\angleKneeLeft -> 7,
	\angleKneeRight -> 8,
	\angleShoulderLeft -> 9,
	\angleShoulderRight -> 10
];
~inSlotArray = Array.fill(11, {arg i; i = 0.5;});

//Synth
~buf = Buffer.alloc(s, s.sampleRate * 8.0, 2);
~synthDef = SynthDef(\ownVocoder, { | amp = 0.2,  qtmIn1 = 0, qtmIn2 = 0, qtmIn3 = 0, qtmIn4 = 0, qtmIn5 = 0|
    var in, chain, chainB, chainC , chainD,  chainE;
	amp.spec = ControlSpec(0.0, 1.0, \db);
	qtmIn1.spec = ControlSpec(0.0, 1.0, \lin);
	qtmIn2.spec = ControlSpec(0.0, 1.0, \lin);
	qtmIn3.spec = ControlSpec(0.0, 1.0, \lin);
	qtmIn4.spec = ControlSpec(0.0, 1.0, \lin);
	qtmIn5.spec = ControlSpec(0.0, 1.0, \lin);
	in = In.ar(8, 1);
    chain = FFT(LocalBuf(2048), in);
    chain = PV_MagFreeze(chain, qtmIn1 > 0.9 );
	chainB = FFT(LocalBuf(2048), in);
	chainB = PV_BinShift(chainB, 5/6 );
	chainB = PV_MagFreeze(chainB, qtmIn2 > 0.7 );
	chainC = FFT(LocalBuf(2048), in);
	chainC = PV_BinShift(chainC, 15/16 );
	chainC = PV_MagFreeze(chainC, qtmIn3 > 0.5 );
	chainD = FFT(LocalBuf(2048), in);
	chainD = PV_BinShift(chainD, 3/5 );
	chainD = PV_MagFreeze(chainD, qtmIn4 > 0.3 );
	chainE = FFT(LocalBuf(2048), in);
	chainE = PV_BinShift(chainE, 6/5);
	Out.ar(0,  amp  *( IFFT(chain).dup + IFFT(chainB).dup + IFFT(chainC).dup + IFFT(chainD).dup) + (IFFT(chainE).dup * qtmIn5));
}).add;

~paramArray = ~synthDef.allControlNames;
~outSpecDict = ~synthDef.specs;
~outSpecArray = [];
~paramArray.do{|item, index|
	~paramArray[index] = item.name.asSymbol;
};

~outSpecDict.do{|item, index|
	~outSpecArray = ~outSpecArray.add( item.value);
};


//Function
~reassignInSlots = {
	~availableInSlots = [];
	~chosenInSlots = [];
	~inSlotArray.do{|value,ind|
		//"Value: " + value + "; ind"
		(value != 0).if{~availableInSlots = ~availableInSlots.add(ind)};
		"inSlotArrray: ".post;
		ind.post;
	};
	if(~availableInSlots.size >= ~paramArray.size){
		while{~chosenInSlots.size < ~paramArray.size}
		{
			~chosenSlot = ~availableInSlots.choose;
			~slotHasAlreadyBeenChosen = false;
			~chosenInSlots.do{|slot| if(slot==~chosenSlot){~slotHasAlreadyBeenChosen = true;}};
			if(~slotHasAlreadyBeenChosen!=true) {~chosenInSlots = ~chosenInSlots.add(~chosenSlot);};
			"while-loop:: ".post;
			"~chosenInSlots.size: ".post;
			~chosenInSlots.size.post;
			";  ~paramArray.size: ".post;
			~paramArray.size.postln;

		};
	}{
		"Not Enough Available Slots: ".post;
		"Size of Available Slots: ".post;
		~availableInSlots.size.postln;
		"Size of ParamArray: ".post;
		~paramArray.size.size.postln;
	}

};

)

//OSC-Com
(



//Recieve from all ports of localhost:
n = NetAddr("127.0.0.1");

OSCdef(\unityIn, {|msg, time, addr, recvPort|
	//msg.postln;
	~inSlotArray.do{|value, index|
		//msg.postln;
		~inSlotArray[index] = ~inSpecArray[index].unmap(msg[index+1]);
		//~inSlotArray[index] = msg[index+1];
	};
}, '/wek/outputs', n); // def style


)


(
m = NetAddr("127.0.0.1", 6969); // send to Unity
~inSlotAssign = Array.fill(~paramArray.size, {0});

~reassignInSlots.value;

~msgs = List.new(size: 20);
~msgs.add("/CubeX");
~paramArray.do{|key, index|
	var msg;
	msg = ("key: " + key + " -- " + ~inNamesDict.findKeyForValue(~chosenInSlots[index]));
//	msg = ("key: " + key + " assigned to -- " + ~inNamesDict.findKeyForValue(~chosenInSlots[index]));
	msg.postln;
	~msgs.add(msg.asString);
	~msgs.postln;

};
m.sendMsg(*~msgs);
~msgs.add("test");
x.free;
x = Synth(\ownVocoder);
Tdef(\parameterAssigner, {
	//x.set(\wet, 0.9);
	loop{
		~paramArray.size.do{|paramIter|
			if(paramIter > 1)
			{x.set(~paramArray[paramIter], ~outSpecArray[paramIter].map(~inSlotArray[~chosenInSlots[paramIter]]));}


		};
		0.01.wait;
	}
});
Tdef(\parameterAssigner).play;
)
s.meter
(
x.free;
Tdef(\parameterAssigner).stop;
)
//spannende Combi:key:  amp  assigned to slot nr.  0
key:  wet  assigned to slot nr.  1
key:  f1  assigned to slot nr.  6
key:  f2  assigned to slot nr.  3
key:  f3  assigned to slot nr.  10
key:  f4  assigned to slot nr.  8
key:  f5  assigned to slot nr.  5

(
var paramIter = 5;
~outSpecArray[paramIter].map(~inSlotArray[~chosenInSlots[paramIter]]).postln;
)



m.sendMsg("/CubeX", 2, "Hallo Welt 1000", "Hallo Welt 7000");
NetAddr

m = NetAddr("127.0.0.1", 6969);
~msg = ~msg.clear;
~msg = ["/CubeX"];
~msg.add("Ans");
m.sendMsg(*~msg);

n = NetAddr("localhost", 12345);
n = s.addr;

// Example sending symbols, integers, and a float
n.sendMsg('/s_new', \default, 2000, 0, s.defaultGroup.nodeID, \freq, 60.midicps);

// The initial forward slash can be omitted
n.sendMsg(\n_set, 2000, \gate, 0);

// Using the performList syntax, you can use an array to store an OSC message



