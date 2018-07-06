const assert = require('chai').assert;
const httpMocks = require("node-mocks-http");
const EventEmitter =  require('events');
const exampleRouteHandler = require("../routes/api/v1/phoneme");

  describe("Test text to phoneme conversion.", () => {
    it("Hello world", function (done) {
      const mockRequest = httpMocks.createRequest({
        method: "GET",
        url: "/?text=Hello&lang=en-US",
      });
      const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
      exampleRouteHandler(mockRequest, mockResponse);
      
      const expectedResponseBody = [ 'HH', 'AH', 'L', 'OW' ];

      mockResponse.on('send', () => {
        assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
        done();
      });
    });

    it("Introduction sentence", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=Hello we are epva4&lang=en-US",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ 'HH', 'AH', 'L', 'OW', '/', 'W', 'IY', '/', 'AA', 'R', 'R', '/', 'EH', 'P', 'V', 'AH', '/', 'F', 'AO' ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      it("Demo sentence", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=The quick brown fox jumps over the lazy dog&lang=en-US",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ 'DH', 'AH', '/', 'K', 'W', 'IH', 'K', '/', 'B', 'R', 'AW', 'N', '/', 'F', 'AA', 'K', 'S', '/', 'JH', 'AH', 'M', 'P', 'S', '/', 'OW', 'V', 'ER', '/', 'DH', 'AH', '/', 'L', 'EY', 'Z', 'IY', '/', 'D', 'AA', 'G' ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      it("Full viseme conversion", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=beat bit bait bet bat pot buy down but bought boat boy book lute bird cute about kisses killer bird butter calor churn wet yet red let head mother could met net sing bottle debt button fin vet thin this sit zoo shin measure pet bet test debt kit get batter Latin queue church judge&lang=en-US",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = ["B" , "IY" , "T" , "/" , "B" , "IH" , "T" , "/" , "B" , "EY" , "T" , "/" , "B" , "EH" , "T" , "/" , "B" , "AE" , "T" , "/" , "P" , "AA" , "T" , "/" , "B" , "AY" , "/" , "D" , "AW" , "N" , "/" , "B" , "AH" , "T", "/" , "B" , "AO" , "T" , "/" , "B" , "OW" , "T" , "/" , "B" , "OY" , "/" , "B" , "UH", "K" , "/" , "L" , "UW" , "T" , "/" , "B" , "ER" , "D" , "/" , "K" , "Y" , "UW" , "T", "/" , "AH", "B" ,  "AW" , "T" , "/" , "K" , "IH", "S" , "IH" , "Z", "/" , "K" , "IH" , "L" , "ER" , "/" , "B" , "ER" , "D" , "/" , "B", "AH" , "T" , "ER" , "/" , "K" , "AE" , "L" , "ER" , "/" , "CH" , "ER" , "N" , "/" , "W" , "EH" , "T" , "/" , "Y" , "EH" , "T" , "/" , "R" , "EH" , "D" , "/" , "L" , "EH" , "T" , "/" , "HH" , "EH" , "D" , "/", "M" , "AH" , "DH" , "ER" , "/" , "K" , "UH" , "D" , "/" , "M" , "EH" , "T" , "/" , "N" , "EH" , "T" , "/" , "S" , "IH" , "NG", "/" , "B" , "AA" , "T" , "AH" , "L" , "/" , "D" , "EH" , "T" , "/" , "B" , "AH", "EN" , "/" , "F" , "IH" , "N" , "/" , "V" , "EH" , "T" ,  "/" , "TH" , "IH" , "N" , "/", "DH" , "IH" , "S", "/" , "S" , "IH" , "T" , "/", "Z" , "UW" ,  "/" , "SH" , "IH" , "N" , "/" , "M" , "EH" , "ZH" , "ER" , "/" , "P" , "EH" , "T" , "/" , "B" , "EH" , "T" , "/" , "T" , "EH" , "S" , "T" , "/", "D" , "EH" , "T" , "/" , "K" , "IH" , "T" , "/" , "G" , "EH" , "T" , "/" , "B" , "AE" , "T" , "ER" , "/" , "L" , "AE" , "T" , "IH" , "NG" , "/" , "K" , "Y" , "UW" , "/" , "CH" , "ER" , "CH" , "/" , "JH" , "AH" , "JH" ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      // 
  
      it("Full english phoneme conversion", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=big dog fish go egg hot jet cat leg bell mad no pie run sun top vet wet yes zip thumb this sing ship chip garage what hat bed if hot up bacon me find no human book moon cow coin car air mirror for burn&lang=en-US",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ "B" , "IH" , "G" , "/" , "D" , "AA" , "G" , "/" , "F" , "IH" , "SH" , "/" , "G" , "OW" , "/" , "EH" , "G" , "/" , "HH" , "AA" , "T" , "/" , "JH" , "EH" , "T" , "/" , "K" , "AE" , "T" , "/" , "L" , "EH" , "G" , "/" , "B" , "EH" , "L" , "/" , "M" , "AE" , "D" , "/" , "N" , "OW" , "/" , "P" , "AY" , "/" , "R" , "AH" , "N" , "/" , "S" , "AH" , "N" , "/" , "T" , "AA" , "P" , "/" , "V" , "EH" , "T" , "/" , "W" , "EH" , "T" , "/" , "Y" , "EH" , "S" , "/" , "Z" , "IH" , "P" , "/" , "TH" , "AH" , "M" , "/" , "DH" , "IH" , "S" , "/" , "S" , "IH" , "NG" , "/" , "SH" , "IH" , "P" , "/" , "CH" , "IH" , "P" , "/" , "G" , "ER" , "R" , "AA" , "ZH" , "/" , "W" , "AH" , "T" , "/" , "HH" , "AE" , "T" , "/" , "B" , "EH" , "D" , "/" , "IH" , "F" , "/" , "HH" , "AA" , "T" , "/" , "AH" , "P" , "/" , "B" , "EY" , "K" , "AH" , "N" , "/" , "M" , "IY" , "/" , "F" , "AY" , "N" , "D" , "/" , "N" , "OW" , "/" , "HH" , "Y" , "UW" , "M" , "AH" , "N" , "/" , "B" , "UH" , "K" , "/" , "M" , "UW" , "NG" , "/" , "K" , "AW" , "/" , "K" , "OY" , "NG" , "/" , "K" , "AA" , "R" , "R" , "/" , "EH" , "R" , "/" , "M" , "IH" , "R" , "ER" , "/" , "F" , "AO" , "R" , "/" , "B" , "ER" , "N" ];
  
        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      it("Full dutch phoneme conversion", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=pot tam kar fiets samen chef chaos bot dam garcon vier zaad journaal gat hand la rij maan na slang jas wie&lang=nl",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ "P" , "AO" , "/" , "T" , "AA" , "M" , "/" , "K" , "AA" , "R" , "/" , "F" , "IY" , "T" , "/" , "S" , "AA_nl" , "M" , "AH" , "N" , "/" , "SH" , "EH" , "F" , "/" , "G_nl" , "AA_nl" , "AO" , "S" , "/" , "B" , "AO" , "/" , "T" , "AA" , "M" , "/" , "G_nl" , "AA" , "R" , "K" , "AO" , "N" , "/" , "V" , "IY" , "R" , "/" , "Z" , "AA_nl" , "T" , "/" , "ZH" , "UW" , "R" , "N" , "AA_nl" , "L" , "/" , "G_nl" , "AA" , "T" , "/" , "HH" , "AA" , "N" , "T" , "/" , "L" , "AA_nl" , "/" , "R" , "EI_nl" , "/" , "M" , "AA_nl" , "N" , "/" , "N" , "AA_nl" , "/" , "S" , "L" , "AA" , "NG" , "/" , "Y" , "AA" , "S" , "/" , "W" , "IY" ];

        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).phonemes, expectedResponseBody);
          done();
        });
      });

      it("Check language warning", function (done) {
        const mockRequest = httpMocks.createRequest({
          method: "GET",
          url: "/?text=hello&lang=af",
        });
        const mockResponse = httpMocks.createResponse({eventEmitter: EventEmitter});
        exampleRouteHandler(mockRequest, mockResponse);
        
        const expectedResponseBody = [ "language not supported by API" ];

        mockResponse.on('send', () => {
          assert.deepEqual(JSON.parse(mockResponse._getData()).warnings, expectedResponseBody);
          done();
        });
      });
});