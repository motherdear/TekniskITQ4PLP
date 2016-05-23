; Execute method invocation on object-like closure
(define (invoke op obj . args)
  (let ([method (obj op)])
       (apply method args)))

; ////////////////////////////////////////////////////////////////////////////

; Creates an object-like bounding-box closure
(define (make-bounding-box bottom-left top-right)
  (letrec ([get-bottom-left (lambda () bottom-left)]
           [get-top-right   (lambda () top-right)]
          )
    (lambda (op)
      (cond [(eq? op 'get-bottom-left) get-bottom-left]
            [(eq? op 'get-top-right) get-top-right]
            [else "op not supported"]))))

; Creates an object-like line closure
(define (make-line from to bb color)
  (letrec ([get-from        (lambda () from)]
           [get-to          (lambda () to)]
           [perimeter  		(lambda ()
                  				; (print (invoke 'get-bottom-left bb))
		           	 			; (print (invoke 'get-top-right bb))
                          		(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4) '(5 5) '(6 6) '(7 7) '(8 8) '(9 9)
                          		      '(10 10) '(11 11) '(12 12) '(13 13) '(14 14) '(15 15) '(16 16) '(17 17) '(18 18) '(19 19)))]
           [draw				(lambda ()
			     					(cons color (perimeter)))]
          )
    (lambda (op)
      (cond [(eq? op 'get-from) get-from]
            [(eq? op 'get-to) get-to]
            [(eq? op 'get-color) color]
            [(eq? op 'perimeter) perimeter]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))

; Creates and object-like circle closure
(define (make-circle center r bb color)
	(letrec ([get-center      	(lambda () center)]
			 [get-r           	(lambda () r)]
			 [perimeter	 		(lambda ()
			           	 			; (print (invoke 'get-bottom-left bb))
			           	 			; (print (invoke 'get-top-right bb))
									(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4)))]
		 	 [fill				(lambda ()
									; call to perimeter
									(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4) '(0 0) '(1 1) '(2 2) '(3 3) '(4 4) '(0 0) '(1 1) '(2 2) '(3 3) '(4 4)))]
			 [draw				(lambda ()
			      					(cons color (perimeter)))]
)
    (lambda (op)
      (cond [(eq? op 'get-center) get-center]
            [(eq? op 'get-r) get-r]
            [(eq? op 'get-color) color]
            [(eq? op 'perimeter) perimeter]
            [(eq? op 'fill) fill]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))

(define (make-rectangle bottom-left top-right bb color)
    (letrec ([get-bottom-left 	(lambda () bottom-left)]
             [get-top-right   	(lambda () top-right)]
             [left     			(make-line bottom-left (list (car bottom-left) (cadr top-right)) bb color)]
	   	     [top				(make-line (list (car bottom-left) (cadr top-right)) top-right bb color)]
	         [right    			(make-line top-right (list (car top-right) (cadr bottom-left)) bb color)]
	   	     [bottom   			(make-line (list (car top-right) (cadr bottom-left)) bottom-left bb color)]
	         [perimeter	 		(lambda ()
	                          		; (apply append (list (invoke 'perimeter left)
     								; 	(invoke 'perimeter top)
     								; 	(invoke 'perimeter right)
     								; 	(invoke 'perimeter bottom bb))))]
     								; (print (invoke 'get-bottom-left bb))
			           	 			; (print (invoke 'get-top-right bb))
                     				(list '(0 0) '(0 1) '(0 2) '(0 3) '(0 4) '(0 5) '(0 6) '(0 7) '(0 8) '(0 9) 
 										  '(0 10) '(0 11) '(0 12) '(0 13) '(0 14) '(0 15) '(0 16) '(0 17) '(0 18) '(0 19)
										  '(0 0) '(1 0) '(2 0) '(3 0) '(4 0) '(5 0) '(6 0) '(7 0) '(8 0) '(9 0) '(10 0) 
										  '(11 0) '(12 0) '(13 0) '(14 0) '(15 0) '(16 0) '(17 0) '(18 0) '(19 0)
										  '(19 0) '(19 1) '(19 2) '(19 3) '(19 4) '(19 5) '(19 6) '(19 7) '(19 8) '(19 9) '(19 10) 
										  '(19 11) '(19 12) '(19 13) '(19 14) '(19 15) '(19 16) '(19 17) '(19 18) '(19 19)
										  '(0 19) '(1 19) '(2 19) '(3 19) '(4 19) '(5 19) '(6 19) '(7 19) '(8 19) '(9 19) '(10 19) 
										  '(11 19) '(12 19) '(13 19) '(14 19) '(15 19) '(16 19) '(17 19) '(18 19) '(19 19)))]
            [fill				(lambda ()
                					(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4) '(0 0) '(1 1) '(2 2) '(3 3) '(4 4) '(0 0) '(1 1) '(2 2) '(3 3) '(4 4)))]
            [draw				(lambda ()
                 					(cons color (perimeter)))]
          )
    (lambda (op)
      (cond [(eq? op 'get-bottom-left) get-bottom-left]
            [(eq? op 'get-top-right) get-top-right]
            [(eq? op 'get-color) color]
            [(eq? op 'perimeter) perimeter]
            [(eq? op 'fill) fill]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))
           
(define (make-draw color objs)
	(letrec ([draw		(lambda () (cons color (apply append (map (lambda (obj) (invoke 'perimeter obj)) objs))))])
	(lambda (op)
		(cond [(eq? op 'draw) draw]
              [else "op not supported"]))))
             
(define (make-fill color obj)
	(letrec ([draw		(lambda () (cons color (invoke 'fill obj)))])
	(lambda (op)
		(cond [(eq? op 'draw) draw]
              [else "op not supported"]))))             
             
(define canvas
  (letrec (
           [get-default-color (lambda () "black")]
           [bounding-box #f]
           [get-bounding-box (lambda () bounding-box)]
           [set-bounding-box (lambda(bottom-left top-right) (set! bounding-box (make-bounding-box bottom-left top-right)))]
          )
    (lambda (op)
       (cond [(eq? op 'get-default-color) get-default-color]
             [(eq? op 'get-bounding-box) get-bounding-box]
             [(eq? op 'set-bounding-box) set-bounding-box]
             [else "op not supported"]))))
            
; ////////////////////////////////////////////////////////////////////////////

; Set the global bounding-box       
(define BOUNDING-BOX
  (lambda (bottom-left top-right)
    (invoke 'set-bounding-box canvas bottom-left top-right)))

; Create a line and compute its coords
(define LINE
  (lambda (from to)
    (let([line  (make-line from to (invoke 'get-bounding-box canvas) (invoke 'get-default-color canvas))]) line)))

; Create a circle and compute its coords
(define CIRCLE
  (lambda (center r)
    (let([circle (make-circle center r (invoke 'get-bounding-box canvas) (invoke 'get-default-color canvas))]) circle)))
   
; Create a rectangle and compute its coords
(define RECTANGLE
  (lambda (bottom-left top-right)
  	(let ([rectangle (make-rectangle bottom-left top-right (invoke 'get-bounding-box canvas) (invoke 'get-default-color canvas))]) rectangle)))
                    
(define DRAW 
	(lambda(color . objs)
		(letrec ([draw (make-draw color objs)]) draw)))
	
(define FILL
	(lambda (color obj)
		(letrec ([fill (make-fill color obj)]) fill)))

(BOUNDING-BOX '(0 0) '(100 100))                    