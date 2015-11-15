(ns GloryOfEmpires
  (:require [clojure.string :as str])
  (:use Utils)
  (:use clojure.test))

(def resources-url "http://www.brotherus.net/ti3/")

(def all-systems
  [ { :image "1planet/Tile-Acheron.gif" }
    { :image "1planet/Tile-Aeon.gif" }
    { :image "1planet/Tile-Aker.gif" }
    { :image "1planet/Tile-Ammit.gif" }
    { :image "1planet/Tile-Amun.gif" }
    { :image "1planet/Tile-Andjety.gif" }
    { :image "1planet/Tile-Anhur.gif" }
    { :image "1planet/Tile-Ankh.gif" }
    { :image "1planet/Tile-Anuket.gif" }
    { :image "1planet/Tile-Apis.gif" }
    { :image "1planet/Tile-Asgard.gif" }
    { :image "1planet/Tile-Asgard_III.gif" }
    { :image "1planet/Tile-Astennu.gif" }
    { :image "1planet/Tile-Aten.gif" }
    { :image "1planet/Tile-Babi.gif" }
    { :image "1planet/Tile-Bakha.gif" }
    { :image "1planet/Tile-Bast.gif" }
    { :image "1planet/Tile-Beriyil.gif" }
    { :image "1planet/Tile-Bes.gif" }
    { :image "1planet/Tile-Capha.gif" }
    { :image "1planet/Tile-Chensit.gif" }
    { :image "1planet/Tile-Chnum.gif" }
    { :image "1planet/Tile-Chuuka.gif" }
    { :image "1planet/Tile-Cicerus.gif" }
    { :image "1planet/Tile-Coruscant.gif" }
    { :image "1planet/Tile-Dedun.gif" }
    { :image "1planet/Tile-Deimo.gif" }
    { :image "1planet/Tile-Discworld.gif" }
    { :image "1planet/Tile-Dune.gif" }
    { :image "1planet/Tile-Elnath.gif" }
    { :image "1planet/Tile-Everra.gif" }
    { :image "1planet/Tile-Faunus.gif" }
    { :image "1planet/Tile-Fiorina.gif" }
    { :image "1planet/Tile-FloydIV.gif" }
    { :image "1planet/Tile-Garbozia.gif" }
    { :image "1planet/Tile-Heimat.gif" }
    { :image "1planet/Tile-Hopes_End.gif" }
    { :image "1planet/Tile-Inaak.gif" }
    { :image "1planet/Tile-Industrex.gif" }
    { :image "1planet/Tile-Iskra.gif" }
    { :image "1planet/Tile-Ithaki.gif" }
    { :image "1planet/Tile-Kanite.gif" }
    { :image "1planet/Tile-Kauket.gif" }
    { :image "1planet/Tile-Kazenoeki.gif" }
    { :image "1planet/Tile-Khepri.gif" }
    { :image "1planet/Tile-Khnum.gif" }
    { :image "1planet/Tile-Klendathu.gif" }
    { :image "1planet/Tile-Kobol.gif" }
    { :image "1planet/Tile-Laurin.gif" }
    { :image "1planet/Tile-Lesab.gif" }
    { :image "1planet/Tile-Lodor.gif" }
    { :image "1planet/Tile-Lv426.gif" }
    { :image "1planet/Tile-Medusa_V.gif" }
    { :image "1planet/Tile-Mehar_Xull.gif" }
    { :image "1planet/Tile-Mirage.gif" }
    { :image "1planet/Tile-Myrkr.gif" }
    { :image "1planet/Tile-Nanan.gif" }
    { :image "1planet/Tile-Natthar.gif" }
    { :image "1planet/Tile-Nef.gif" }
    { :image "1planet/Tile-Nexus.gif" }
    { :image "1planet/Tile-Niiwa-Sei.gif" }
    { :image "1planet/Tile-Pakhet.gif" }
    { :image "1planet/Tile-Parzifal.gif" }
    { :image "1planet/Tile-Perimeter.gif" }
    { :image "1planet/Tile-Petbe.gif" }
    { :image "1planet/Tile-Primor.gif" }
    { :image "1planet/Tile-Ptah.gif" }
    { :image "1planet/Tile-Qetesh.gif" }
    { :image "1planet/Tile-Quann.gif" }
    { :image "1planet/Tile-Radon.gif" }
    { :image "1planet/Tile-Saudor.gif" }
    { :image "1planet/Tile-Sem-Lore.gif" }
    { :image "1planet/Tile-Shai.gif" }
    { :image "1planet/Tile-Shool.gif" }
    { :image "1planet/Tile-Solitude.gif" }
    { :image "1planet/Tile-Sulako.gif" }
    { :image "1planet/Tile-Suuth.gif" }
    { :image "1planet/Tile-Tarmann.gif" }
    { :image "1planet/Tile-Tefnut.gif" }
    { :image "1planet/Tile-Tempesta.gif" }
    { :image "1planet/Tile-Tenenit.gif" }
    { :image "1planet/Tile-Theom.gif" }
    { :image "1planet/Tile-Thibah.gif" }
    { :image "1planet/Tile-Ubuntu.gif" }
    { :image "1planet/Tile-Vefut_II.gif" }
    { :image "1planet/Tile-Wadjet.gif" }
    { :image "1planet/Tile-Wellon.gif" }
    { :image "1planet/Tile-Wepwawet.gif" } ] )

(def tile-height 376)
(def tile-width 432)

(defn- coord-logical-to-screen [ { { x :x y :y } :logical-pos :as piece } ]
  (merge piece
         { :screen-pos
          { :x (* x tile-width 0.75)
            :y (* tile-height (+ (* x 0.5) y)) }} ))

(defn- tile-on-table? [ piece map-size ]
  (let [ { { x :x y :y } :screen-pos } (coord-logical-to-screen piece)
         distance (Math/sqrt (+ (* x x) (* y y))) ]
    (< distance (* tile-height map-size 1.01 ))))

(defn range2d [ range1 range2 ]
  (let [ combine (fn [value1] (map #(vector value1 %) range2)) ]
    (mapcat combine range1)))

(defn random-system [ x y ]
  { :logical-pos { :x x :y y } :system (rand-nth all-systems) } )

(defn make-random-map [ rings ]
  (let [ x-range (range (- rings) (inc rings))
         y-range (range (- rings) (inc rings))
         coords (range2d x-range y-range) ]
    (->> coords
        (map (fn [ [x y] ] (random-system x y) ))
        (filter #(tile-on-table? % rings)) )))


;------------------ to svg ------------------------

(defn- coords-to-screen-raw [ map-pieces ]
  (map coord-logical-to-screen map-pieces) )

(defn- min-coords [ map-pieces ]
  (let [ screen-positions (map :screen-pos map-pieces) ]
  { :x (apply min (map :x screen-positions)) :y (apply min (map :y screen-positions)) } ))

; TODO: overload + and - for them
(defn add-pos [ { x1 :x y1 :y } { x2 :x y2 :y } ] { :x (+ x1 x2) :y (+ y1 y2) } )
(defn sub-pos [ { x1 :x y1 :y } { x2 :x y2 :y } ] { :x (- x1 x2) :y (- y1 y2) } )

(defn- normalize-screen-coords [ pieces ]
  (let [ min-pos (min-coords pieces)
         normalize-piece (fn [ { pos :screen-pos :as piece } ]
           (merge piece { :screen-pos (sub-pos pos min-pos) } )) ]
    (map normalize-piece pieces)))

(defn coords-to-screen [ map-pieces ]
  (normalize-screen-coords (coords-to-screen-raw map-pieces)))

(defn- piece-to-svg [ { { x :x y :y } :screen-pos system :system :as tile } ]
  [ :image { :x (int x) :y (int y) :width tile-width :height tile-height
             "xlink:href" (str resources-url "Tiles/" (system :image)) } ] )

(defn map-to-svg [ map-pieces ]
  [ :svg { :width 1500, :height 1000, "xmlns:xlink" "http://www.w3.org/1999/xlink" }
    (concat [ :g { :transform "scale(0.25)" } ]
      (map piece-to-svg (coords-to-screen map-pieces))) ] )

;----------------- to xml string -------------------------

(defn- key-to-str [ key ]
  (cond
    (string? key) key
    (keyword? key) (name key)
    :else (pr-str key)))

(defn- attr-to-str [ [ key value ] ]
  (str (key-to-str key) "=\"" value "\"" ))

(defn- attrs-to-str [ attrs ]
  (if (empty? attrs) ""
    (str " " (str/join " " (map attr-to-str attrs)))))

(def xml-to-text)

(defn- content-to-str [ content ind ]
  (str/join "" (map #(xml-to-text % ind) content)))

(defn xml-to-text
  ( [ element ] (xml-to-text element 0) )
  ( [ [ tag attrs & content ] indent ]
    (str (indent-str indent) "<" (name tag) (attrs-to-str attrs)
         (if (empty? content) "/>"
           (str ">" (content-to-str content (inc indent)) (indent-str indent) "</" (name tag) ">" )))))

;-----------------------------------------------------------------------

(def a-map (make-random-map 2 ))

(println (pretty-pr a-map))

(println "coords-to-screen-raw")
(println (coords-to-screen-raw a-map))

(println "min-coords")
(println (min-coords (coords-to-screen-raw a-map)))

(println "coords-to-screen")
(println (coords-to-screen a-map))

(println "xml-to-text")
(println (xml-to-text (map-to-svg a-map)))

;(spit "map.svg" (xml-to-text (map-to-svg (make-random-map 1 1 ))))

(deftest map-test
  (are [ expected calculated ] (= expected calculated)
     { :x 0.0 :y 0.0 } (coord-logical-to-screen { :x 0 :y 0 } )
     9 (count (range2d (range 3) (range 3)))))

(clojure.main/repl)

(run-tests)

