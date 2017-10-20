// Changes:
// David Garcia | OCT-19-2017 | Rename/Change namespaces, restyle file, fix any complition issues as applicable
// 
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Lucene.Net.Documents;
using Lucene.Net.Linq.Tests.Integration;
using Lucene.Net.Store;
using Lucene.Net.Util;
using NUnit.Framework;
using Sprockets.Lucene.Net.Linq;
using Sprockets.Lucene.Net.Linq.Fluent;
using Sprockets.Lucene.Net.Linq.Mapping;

namespace Lucene.Net.Linq.Tests.Samples {
    public class PropertyChangeExample {
        /// <summary>
        ///     Note: to work propery the PropertyChanged event is only fired when
        ///     a property changes from a non-null value. This way the event will not
        ///     fire when the entity is first being instantiated.
        /// </summary>
        public class ExampleEntity : INotifyPropertyChanged {
            private string _id;
            private string _name;

            public string Id {
                get => _id;
                set {
                    var prev = _id;
                    _id = value;
                    if (prev != null && !Equals(value, prev))
                        OnPropertyChanged("Id");
                }
            }

            public string Name {
                get => _name;
                set {
                    var prev = _name;
                    _name = value;
                    if (prev != null && !Equals(value, prev))
                        OnPropertyChanged("Name");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName) {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class PropertyChangedModificationDetector<T> : IDocumentModificationDetector<T>
            where T : INotifyPropertyChanged, new() {
            private readonly ISet<T> dirtyItems = new HashSet<T>();

            public IEnumerable<T> DirtyItems => dirtyItems;

            public bool IsModified(T item, Document document) {
                return dirtyItems.Contains(item);
            }

            public T Factory() {
                var entity = new T();
                entity.PropertyChanged += MarkDirty;
                return entity;
            }

            private void MarkDirty(object sender, PropertyChangedEventArgs e) {
                dirtyItems.Add((T) sender);
            }
        }

        [TestFixture]
        public class Tests : IntegrationTestBase {
            [SetUp]
            public override void InitializeLucene() {
                directory = new RAMDirectory();
                provider = new LuceneDataProvider(directory, version);

                modificationDetector = new PropertyChangedModificationDetector<ExampleEntity>();
                map = new ClassMap<ExampleEntity>(Version.LUCENE_30);
                map.Key(e => e.Id);
                map.Property(e => e.Name);

                AddDocument(new ExampleEntity {Id = "entity 1", Name = "default"});
            }

            private PropertyChangedModificationDetector<ExampleEntity> modificationDetector;
            private ClassMap<ExampleEntity> map;

            [Test]
            public void DontFlushUnmodifiedDocument() {
                var session = provider.OpenSession(modificationDetector.Factory,
                    map.ToDocumentMapper(),
                    modificationDetector);

                using (session) {
                    Assert.That(session.Query().First().Name, Is.EqualTo("default"));
                }

                Assert.That(modificationDetector.DirtyItems, Is.Empty);
            }

            [Test]
            public void FlushModifiedDocument() {
                var session = provider.OpenSession(modificationDetector.Factory,
                    map.ToDocumentMapper(),
                    modificationDetector);

                using (session) {
                    session.Query().Single().Name = "updated";
                }

                Assert.That(provider.AsQueryable<ExampleEntity>().Single().Name, Is.EqualTo("updated"));
            }
        }
    }
}
