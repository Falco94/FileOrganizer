using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrganizer.Service
{
    public class PropertyMapper
    {
        private Queue<MappingQueueMember<object, object>> _queue = new Queue<MappingQueueMember<object, object>>();

        public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
        where TTarget : new()
        {
            if (source == null || target == null)
                return new TTarget();

            var sourceProperties = source.GetType().GetProperties();
            var targetProperties = target.GetType().GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                foreach (var targetProperty in targetProperties)
                {
                    if (sourceProperty.Name == targetProperty.Name)
                    {
                        targetProperty.SetValue(target, sourceProperty.GetValue(source));
                        break;
                    }
                }
            }

            return target;
        }

        private void Map(MappingQueueMember<object, object> member)
        {
            Map(member.Source, member.Target);
        }

        public void AddToMappingQueue<TSource, TTarget>(TSource source, TTarget target)
        {
            _queue.Enqueue(new MappingQueueMember<object, object>(source,target));
        }

        public void ExecuteQueue()
        {
            foreach (var queueMember in _queue)
            {
                Map(queueMember);
            }

            _queue.Clear();
        }

        private class MappingQueueMember<TSource, TTarget>
        {
            private TSource _source;
            private TTarget _target;

            public MappingQueueMember(TSource source, TTarget target)
            {
                Source = source;
                Target = target;
            }

            public TSource Source
            {
                get { return _source; }
                set { _source = value; }
            }

            public TTarget Target
            {
                get { return _target; }
                set { _target = value; }
            }
        }
    }
}
