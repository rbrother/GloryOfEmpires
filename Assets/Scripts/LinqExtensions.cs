using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Brotherus {
    public static class LinqExtensions {
        public static TSource MinBy<TSource, TKey>( this IEnumerable<TSource> source,
            Func<TSource, TKey> selector ) {
            return source.MinBy( selector, Comparer<TKey>.Default );
        }

        public static TSource MinBy<TSource, TKey>( this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer ) {
            using ( IEnumerator<TSource> sourceIterator = source.GetEnumerator( ) ) {
                if ( !sourceIterator.MoveNext( ) ) {
                    throw new InvalidOperationException( "Sequence was empty" );
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector( min );
                while ( sourceIterator.MoveNext( ) ) {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector( candidate );
                    if ( comparer.Compare( candidateProjected, minKey ) < 0 ) {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }
    }
}
