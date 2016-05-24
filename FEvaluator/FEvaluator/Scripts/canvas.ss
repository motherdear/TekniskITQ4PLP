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
                  				(define (bresenham from to)
									(let* ([dx		(- (car to) (car from))]
									       [dy		(- (cadr to) (cadr from))]
									       [steep?  (> (abs dy) (abs dx))]
									       [x1		(if steep? (cadr from) (car from))]
									       [y1		(if steep? (car from) (cadr from))]
									       [x2		(if steep? (cadr to) (car to))]
									       [y2		(if steep? (car to) (cadr to))]
									       [rx1		(if (> x1 x2) x2 x1)]
									       [rx2		(if (> x1 x2) x1 x2)]
									       [ry1		(if (> x1 x2) y2 y1)]
									       [ry2		(if (> x1 x2) y1 y2)]
									       [rdx		(- rx2 rx1)]
									       [rdy		(- ry2 ry1)]
									       [error	(floor (/ rdx 2.0))]
									       [ystep	(if (< ry1 ry2) 1 -1)]
									      )
								       (define (bresenham-rec x1 x2 steep? y ystep error dx dy)
									       		
									       		(if (> x1 x2) '()
									       		    (let* ([new-error (- error (abs dy))]
									       		           [new-y	  (if (< new-error 0) (+ y ystep) y)]
									       		           [rerror	  (if (< new-error 0) (+ dx new-error) new-error)]
									       		          )
									       		         
									       		         (append (if steep? (list (list y x1)) (list (list x1 y))) (bresenham-rec (+ x1 1) x2 steep? new-y ystep rerror dx dy ))
									       		     )
									       		)
									       		
								       	) (bresenham-rec rx1 rx2 steep? ry1 ystep error rdx rdy))) (bresenham from to))]
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
									(define (midpoint center r)
										(let ([x 0] 
										      [y r]
										      [dp (- 1 r)]
										     )
											(define (midpoint-rec center x y dp)
												(if (> x y) 
												    '()
												    (let*   ([x0 		(car center)]
										                     [y0		(cadr center)]
										                     [new-x		(+ x 1)]
										      	 			 [new-y		(if (< dp 0) y (- y 1))]
										      	 			 [new-dp 	(if (< dp 0) (+ dp (* 2 new-x) 3) (- (+ dp (* 2 new-x)) (+ (* 2 new-y) 5)))]
										     				)
											    				(append (list (list (+ x0 new-x) (+ y0 new-y)) 
											    				              (list (- x0 new-x) (+ y0 new-y))
											    				              (list (+ x0 new-x) (- y0 new-y))
											    				              (list (- x0 new-x) (- y0 new-y))
											    				              (list (+ x0 new-y) (+ y0 new-x))
											    				              (list (- x0 new-y) (+ y0 new-x))
											    				              (list (+ x0 new-y) (- y0 new-x))
											    				              (list (- x0 new-y) (- y0 new-x))
											    				              ) (midpoint-rec center new-x new-y new-dp))))
										    )(midpoint-rec center x y dp)
										   ))
										   (midpoint center r)
									)]
		 	 [fill				(lambda ()
									; call to perimeter
									(define (midpoint center r)
										(let ([x 0] 
										      [y r]
										      [dp (- 1 r)]
										     )
											(define (midpoint-rec center x y dp)
												(if (> x y) 
												    '()
												    (let*   ([x0 		(car center)]
										                     [y0		(cadr center)]
										                     [new-x		(+ x 1)]
										      	 			 [new-y		(if (< dp 0) y (- y 1))]
										      	 			 [new-dp 	(if (< dp 0) (+ dp (* 2 new-x) 3) (- (+ dp (* 2 new-x)) (+ (* 2 new-y) 5)))]
										     				)
											    				; Replace with calls to make-line
											    				(append (list  
											    				              (invoke 'perimeter (make-line (list (+ x0 new-x) (+ y0 new-y)) (list (- x0 new-x) (+ y0 new-y)) bb color))
											    				              (invoke 'perimeter (make-line (list (+ x0 new-x) (- y0 new-y)) (list (- x0 new-x) (- y0 new-y)) bb color))
											    				              (invoke 'perimeter (make-line (list (+ x0 new-y) (+ y0 new-x)) (list (- x0 new-y) (+ y0 new-x)) bb color))
											    				              (invoke 'perimeter (make-line (list (+ x0 new-y) (- y0 new-x)) (list (- x0 new-y) (- y0 new-x)) bb color))
											    				              ) (midpoint-rec center new-x new-y new-dp))))
										    ) (apply append (midpoint-rec center x y dp))
										   ))
										   (midpoint center r)
								)]
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
	                          		(apply append (list (invoke 'perimeter left)
     									(invoke 'perimeter top)
     									(invoke 'perimeter right)
     									(invoke 'perimeter bottom))))]
     								; (print (invoke 'get-bottom-left bb))
			           	 			; (print (invoke 'get-top-right bb))
            ;          				(list '(0 0) '(0 1) '(0 2) '(0 3) '(0 4) '(0 5) '(0 6) '(0 7) '(0 8) '(0 9) 
 										 ; '(0 10) '(0 11) '(0 12) '(0 13) '(0 14) '(0 15) '(0 16) '(0 17) '(0 18) '(0 19)
										  ;'(0 0) '(1 0) '(2 0) '(3 0) '(4 0) '(5 0) '(6 0) '(7 0) '(8 0) '(9 0) '(10 0) 
										  ;'(11 0) '(12 0) '(13 0) '(14 0) '(15 0) '(16 0) '(17 0) '(18 0) '(19 0)
										  ;'(19 0) '(19 1) '(19 2) '(19 3) '(19 4) '(19 5) '(19 6) '(19 7) '(19 8) '(19 9) '(19 10) 
										  ;'(19 11) '(19 12) '(19 13) '(19 14) '(19 15) '(19 16) '(19 17) '(19 18) '(19 19)
										  ;'(0 19) '(1 19) '(2 19) '(3 19) '(4 19) '(5 19) '(6 19) '(7 19) '(8 19) '(9 19) '(10 19) 
										  ;'(11 19) '(12 19) '(13 19) '(14 19) '(15 19) '(16 19) '(17 19) '(18 19) '(19 19)))]
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