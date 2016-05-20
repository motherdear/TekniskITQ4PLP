; Execute method invocation on object-like closure
(define (invoke op obj . args)
	(let ([method (obj op)])
	     (apply method args)))

; Creates an object-like bounding-box closure
(define (make-bounding-box bottom-left top-right)
  (letrec ((get-bottom-left (lambda () bottom-left))
           (get-top-right   (lambda () top-right))
          )
    (lambda (op)
      (cond ((eq? op 'get-bottom-left) get-bottom-left)
            ((eq? op 'get-top-right) get-top-right)
            (else "op not supported")))))

; Creates an object-like line closure
(define (make-line from to)
  (letrec ((color           (lambda () "black"))
           (get-from        (lambda () from))
           (get-to          (lambda () to))
           (compute-coord	((lambda (self) (self self))
 								(lambda (compute)
   									(lambda (bb)
                    					(print (invoke 'get-bottom-left bb))
                    					(print (invoke 'get-top-right bb))
                    					(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4))))))
           (draw			(lambda (bb) (cons (color) (compute-coord bb))))
          )
    (lambda (op)
      (cond ((eq? op 'get-from) get-from)
            ((eq? op 'get-to) get-to)
            ((eq? op 'get-color) color)
            ((eq? op 'draw) draw)
            (else "op not supported")))))

; Creates and object-like circle closure
(define (make-circle center r)
  (letrec ((color           (lambda () "black"))
           (get-center      (lambda () center))
           (get-r           (lambda () r))
           (compute-coord	((lambda (self) (self self))
 								(lambda (compute-coord)
   									(lambda (bb)
                    					(print (invoke 'get-bottom-left bb))
                    					(print (invoke 'get-top-right bb))
                    					(list '(0 0) '(1 1) '(2 2) '(3 3) '(4 4))))))
           (draw			(lambda (bb) (cons (color) (compute-coord bb))))
          )
    (lambda (op)
      (cond ((eq? op 'get-center) get-center)
            ((eq? op 'get-r) get-r)
            ((eq? op 'get-color) color)
            ((eq? op 'draw) draw)
            (else "op not supported")))))

; Global used for storing program state
(define canvas
  (letrec ((bounding-box #f)
           (get-bounding-box (lambda () bounding-box))
           (set-bounding-box (lambda(bottom-left top-right) (set! bounding-box (make-bounding-box bottom-left top-right))))
          )
    (lambda (op)
       (cond ((eq? op 'get-bounding-box) get-bounding-box)
             ((eq? op 'set-bounding-box) set-bounding-box)
             (else "op not supported")))))

; Set the global bounding-box       
(define BOUNDING-BOX
	(lambda (bottom-left top-right)
		(invoke 'set-bounding-box canvas bottom-left top-right)))

; Create a line and compute its coords
(define LINE
	(lambda (from to)
		(let([line	(make-line from to)]) (invoke 'draw line (invoke 'get-bounding-box canvas)))))

; Create a circle and compute its coords
(define CIRCLE
	(lambda (center r)
		(let([circle (make-circle center r)]) (invoke 'draw circle (invoke 'get-bounding-box canvas)))))
