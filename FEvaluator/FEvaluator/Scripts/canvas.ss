; Execute method invocation on object-like closure
(define (invoke op obj . args)
  (let ([method (obj op)])
       (apply method args)))

; ////////////////////////////////////////////////////////////////////////////

; Creates an object-like bounding-box closure
(define (make-bounding-box bottom-left top-right color)
  (letrec ([get-bottom-left (lambda () bottom-left)]
           [get-top-right   (lambda () top-right)]
           [draw            (lambda () (cons color (invoke 'perimeter (make-rectangle bottom-left top-right ""))))]
          )
    (lambda (op)
      (cond [(eq? op 'get-bottom-left) get-bottom-left]
            [(eq? op 'get-top-right) get-top-right]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))

; Creates an object-like line closure
(define (make-line from to color)
  (letrec ([get-from        (lambda () from)]
           [get-to          (lambda () to)]
           [perimeter       (lambda ()
                                (define (bresenham from to)
                                    (let* ([dx      (- (car to) (car from))]
                                           [dy      (- (cadr to) (cadr from))]
                                           [steep?  (> (abs dy) (abs dx))]
                                           [x1      (if steep? (cadr from) (car from))]
                                           [y1      (if steep? (car from) (cadr from))]
                                           [x2      (if steep? (cadr to) (car to))]
                                           [y2      (if steep? (car to) (cadr to))]
                                           [rx1     (if (> x1 x2) x2 x1)]
                                           [rx2     (if (> x1 x2) x1 x2)]
                                           [ry1     (if (> x1 x2) y2 y1)]
                                           [ry2     (if (> x1 x2) y1 y2)]
                                           [rdx     (- rx2 rx1)]
                                           [rdy     (- ry2 ry1)]
                                           [error   (floor (/ rdx 2.0))]
                                           [ystep   (if (< ry1 ry2) 1 -1)]
                                          )
                                       (define (bresenham-rec x1 x2 steep? y ystep error dx dy)
                                                
                                                (if (> x1 x2) '()
                                                    (let* ([new-error (- error (abs dy))]
                                                           [new-y     (if (< new-error 0) (+ y ystep) y)]
                                                           [rerror    (if (< new-error 0) (+ dx new-error) new-error)]
                                                          )
                                                         
                                                         (append (if steep? (list (list y x1)) (list (list x1 y))) (bresenham-rec (+ x1 1) x2 steep? new-y ystep rerror dx dy))
                                                     )
                                                )
                                                
                                        ) (bresenham-rec rx1 rx2 steep? ry1 ystep error rdx rdy))) (bresenham from to))]
           [draw                (lambda ()
                                    (cons color (invoke 'clip canvas (perimeter))))]
          )
    (lambda (op)
      (cond [(eq? op 'get-from) get-from]
            [(eq? op 'get-to) get-to]
            [(eq? op 'get-color) color]
            [(eq? op 'perimeter) perimeter]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))

; Creates and object-like circle closure
(define (make-circle center r color)
    (letrec ([get-center        (lambda () center)]
             [get-r             (lambda () r)]
             [perimeter         (lambda ()
                                    (define (midpoint center r)
                                        (let ([x 0] 
                                              [y r]
                                              [dp (- 1 r)]
                                             )
                                            (define (midpoint-rec center x y dp)
                                                (if (> x y) 
                                                    '()
                                                    (let*   ([x0        (car center)]
                                                             [y0        (cadr center)]
                                                             [new-x     (+ x 1)]
                                                             [new-y     (if (< dp 0) y (- y 1))]
                                                             [new-dp    (if (< dp 0) (+ dp (* 2 new-x) 3) (- (+ dp (* 2 new-x)) (+ (* 2 new-y) 5)))]
                                                            )
                                                                (append (list (list (+ x0 x) (+ y0 new-y)) 
                                                                              (list (- x0 x) (+ y0 new-y))
                                                                              (list (+ x0 x) (- y0 new-y))
                                                                              (list (- x0 x) (- y0 new-y))
                                                                              (list (+ x0 new-y) (+ y0 x))
                                                                              (list (- x0 new-y) (+ y0 x))
                                                                              (list (+ x0 new-y) (- y0 x))
                                                                              (list (- x0 new-y) (- y0 x))
                                                                              ) (midpoint-rec center new-x new-y new-dp))))
                                            )(midpoint-rec center x y dp)
                                           ))
                                           (midpoint center r)
                                    )]
             [fill              (lambda ()
                                    (define (midpoint center r)
                                        (let ([x 0] 
                                              [y r]
                                              [dp (- 1 r)]
                                             )
                                            (define (midpoint-rec center x y dp)
                                                (if (> x y) 
                                                    '()
                                                    (let*   ([x0        (car center)]
                                                             [y0        (cadr center)]
                                                             [new-x     (+ x 1)]
                                                             [new-y     (if (< dp 0) y (- y 1))]
                                                             [new-dp    (if (< dp 0) (+ dp (* 2 new-x) 3) (- (+ dp (* 2 new-x)) (+ (* 2 new-y) 5)))]
                                                            )
                                                                ; Replace with calls to make-line
                                                                (append (list  
                                                                              (invoke 'perimeter (make-line (list (+ x0 x) (+ y0 new-y)) (list (- x0 x) (+ y0 new-y)) color))
                                                                              (invoke 'perimeter (make-line (list (+ x0 x) (- y0 new-y)) (list (- x0 x) (- y0 new-y)) color))
                                                                              (invoke 'perimeter (make-line (list (+ x0 new-y) (+ y0 x)) (list (- x0 new-y) (+ y0 x)) color))
                                                                              (invoke 'perimeter (make-line (list (+ x0 new-y) (- y0 x)) (list (- x0 new-y) (- y0 x)) color))
                                                                              ) (midpoint-rec center new-x new-y new-dp))))
                                            ) (apply append (midpoint-rec center x y dp))
                                           ))
                                           (invoke 'clip canvas (midpoint center r))
                                )]
             [draw              (lambda ()
                                    (cons color  (invoke 'clip canvas (perimeter))))]
            )
    (lambda (op)
      (cond [(eq? op 'get-center) get-center]
            [(eq? op 'get-r) get-r]
            [(eq? op 'get-color) color]
            [(eq? op 'perimeter) perimeter]
            [(eq? op 'fill) fill]
            [(eq? op 'draw) draw]
            [else "op not supported"]))))

(define (make-rectangle bottom-left top-right color)
    (letrec ([get-bottom-left   (lambda () bottom-left)]
             [get-top-right     (lambda () top-right)]
             [top-left          (list (car bottom-left) (cadr top-right))]
             [bottom-right      (list (car top-right) (cadr bottom-left))]
             [perimeter         (lambda ()
                                    (apply append (list (invoke 'perimeter (make-line bottom-left top-left color))
                                        (invoke 'perimeter (make-line top-left top-right color))
                                        (invoke 'perimeter (make-line top-right bottom-right color))
                                        (invoke 'perimeter (make-line bottom-right bottom-left color)))))]
                                    
            [fill               (lambda ()
                                    (define fill-rectangle
                                        (let ([target-y (cadr top-left)])
                                             (define (fill-rectangle-rec from to target-y)
                                                 (if (> (cadr from) target-y) '()
                                                     (let* ([new-from    (list (car from) (+ (cadr from) 1))]
                                                            [new-to      (list (car to) (+ (cadr to) 1))]
                                                           )
                                                        (append (invoke 'perimeter (make-line from to color)) (fill-rectangle-rec new-from new-to target-y))
                                                      )
                                                 )
                                             ) (fill-rectangle-rec bottom-left bottom-right target-y)) 
                                    ) (invoke 'clip canvas fill-rectangle)
                                        
                                )]
            [draw               (lambda ()
                                    (cons color (invoke 'clip canvas (perimeter))))]
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
    (letrec ([draw      (lambda () (cons color (apply append (map (lambda (obj) (invoke 'perimeter obj)) objs))))])
    (lambda (op)
        (cond [(eq? op 'draw) draw]
              [else "op not supported"]))))
             
(define (make-fill color obj)
    (letrec ([draw      (lambda () (cons color (invoke 'fill obj)))])
    (lambda (op)
        (cond [(eq? op 'draw) draw]
              [else "op not supported"]))))             
             
(define canvas
  (letrec (
           [get-default-color           (lambda () "Black")]
           [get-default-bounding-color  (lambda () "DarkGray")]
           [bounding-box #f]
           [get-bounding-box            (lambda () bounding-box)]
           [set-bounding-box            (lambda(bb) (set! bounding-box bb))]
           [clip                        (lambda (perimeter)
                                            (let([bot-x (car (invoke 'get-bottom-left (get-bounding-box)))]
                                                 [bot-y (cadr (invoke 'get-bottom-left (get-bounding-box)))]
                                                 [top-x (car (invoke 'get-top-right (get-bounding-box)))]
                                                 [top-y (cadr (invoke 'get-top-right (get-bounding-box)))]
                                                )
                                                 ;(print "bot-x=" bot-x " bot-y=" bot-y " top-x=" top-x " top-y=" top-y)
                                                 (filter (lambda (pixel) 
                                                    (let([x     (car pixel)]
                                                         [y     (cadr pixel)]) 
                                                       (not (or (<= x bot-x) (>= x top-x) (<= y bot-y) (>= y top-y))))) perimeter)
                                                                  
                                            ))]
          )
    (lambda (op)
       (cond [(eq? op 'get-default-color) get-default-color]
             [(eq? op 'get-default-bounding-color) get-default-bounding-color]
             [(eq? op 'get-bounding-box) get-bounding-box]
             [(eq? op 'set-bounding-box) set-bounding-box]
             [(eq? op 'clip) clip]
             [else "op not supported"]))))
            
; ////////////////////////////////////////////////////////////////////////////

; Set the global bounding-box       
(define BOUNDING-BOX
  (lambda (bottom-left top-right)
    (let ([bb (make-bounding-box bottom-left top-right (invoke 'get-default-bounding-color canvas))])
    (invoke 'set-bounding-box canvas bb)
    	(when (or (< (car bottom-left) 0) (< (car top-right) 0) (< (cadr bottom-left) 0) (< (cadr top-right) 0))
      			(raise "Invalid Arguments: Negative Arguments typed"))
     bb )))

; Create a line and compute its coords
(define LINE
  (lambda (from to)
    (let([line  (make-line from to (invoke 'get-default-color canvas))]) 
               	(when (or (< (car from) 0) (< (car to) 0) (< (cadr from) 0) (< (cadr to) 0))
      			(raise "Invalid Arguments: Negative Arguments typed"))	
               line)))

; Create a circle and compute its coords
(define CIRCLE
  (lambda (center r)
    (let([circle (make-circle center r (invoke 'get-default-color canvas))])
    	(when (or (< (car center) 0) (< (cadr center) 0) (< r 0))
      			(raise "Invalid Arguments: Negative Arguments typed"))
    		 circle)))
   
; Create a rectangle and compute its coords
(define RECTANGLE
  (lambda (bottom-left top-right)
    (let ([rectangle (make-rectangle bottom-left top-right (invoke 'get-default-color canvas))])
    	(when (or (< (car bottom-left) 0) (< (car top-right) 0) (< (cadr bottom-left) 0) (< (cadr top-right) 0))
      			(raise "Invalid Arguments: Negative Arguments typed"))
     rectangle)))
                    
(define DRAW 
    (lambda(color . objs)
        (letrec ([draw (make-draw color objs)]) draw)))
    
(define FILL
    (lambda (color obj)
        (letrec ([fill (make-fill color obj)]) fill)))