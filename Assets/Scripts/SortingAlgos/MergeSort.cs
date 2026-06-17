using System;

namespace Sowtank.Collections.Sorting
{
    // MERGE SORT — "Ordenamiento por mezcla"
    // Nombre: divide el arreglo en mitades hasta tener subarreglos de 1 elemento
    // (trivialmente ordenados) y luego los MEZCLA (merge) de vuelta en orden.
    // Estrategia: Divide y Vencerás (Divide & Conquer).
    //
    // Cuándo usarlo:
    //   ✓ Cuando se necesita rendimiento GARANTIZADO — siempre O(n log n)
    //   ✓ Ordenamiento estable para datos con múltiples claves (ej: ordenar por apellido,
    //     luego por nombre, manteniendo el orden anterior)
    //   ✓ Listas enlazadas (no necesita acceso aleatorio, a diferencia de QuickSort)
    //   ✗ Memoria limitada — requiere O(n) espacio extra
    //
    // Mejor: O(n log n) | Promedio: O(n log n) | Peor: O(n log n) | Espacio: O(n) | Estable: Sí
    public static class MergeSort
    {
        public static void Sort<T>(T[] arr) where T : IComparable<T>
        {
            if (arr.Length <= 1) return;
            SortRecursive(arr, 0, arr.Length - 1);
        }

        private static void SortRecursive(T[] arr, int left, int right)
        {
            if (left >= right) return; // caso base: subarreglo de 1 elemento

            int mid = left + (right - left) / 2; // evita overflow vs (left+right)/2

            SortRecursive(arr, left, mid);       // ordena mitad izquierda
            SortRecursive(arr, mid + 1, right);  // ordena mitad derecha
            Merge(arr, left, mid, right);         // mezcla las dos mitades ordenadas
        }

        // fusiona arr[left..mid] y arr[mid+1..right] en orden
        private static void Merge(T[] arr, int left, int mid, int right)
        {
            int leftSize  = mid - left + 1;
            int rightSize = right - mid;

            // copias temporales de cada mitad
            T[] leftArr  = new T[leftSize];
            T[] rightArr = new T[rightSize];

            Array.Copy(arr, left,     leftArr,  0, leftSize);
            Array.Copy(arr, mid + 1,  rightArr, 0, rightSize);

            int i = 0, j = 0, k = left;

            // mezcla comparando el frente de cada mitad
            while (i < leftSize && j < rightSize)
            {
                // <= garantiza estabilidad: ante empate, toma el elemento izquierdo primero
                if (leftArr[i].CompareTo(rightArr[j]) <= 0)
                    arr[k++] = leftArr[i++];
                else
                    arr[k++] = rightArr[j++];
            }

            // copia los elementos restantes (solo uno de los dos tendrá elementos)
            while (i < leftSize)  arr[k++] = leftArr[i++];
            while (j < rightSize) arr[k++] = rightArr[j++];
        }

        // Caso de uso: ordenar registros de empleados primero por departamento, luego por nombre
        // MergeSort preserva el orden relativo de registros con la misma clave (estable)
        public static string[] UseCaseDemo()
        {
            string[] employees = { "Carlos-IT", "Ana-HR", "Bob-IT", "Diana-HR", "Eve-IT" };
            Sort(employees); // orden lexicográfico estable
            return employees; // → Ana-HR, Bob-IT, Carlos-IT, Diana-HR, Eve-IT
        }
    }
}
